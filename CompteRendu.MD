# Compte rendu du projet Love Letter

* Jules Van Assche 
* Jorick Nuss
* François Bock
* Adrien Roberty
* Paul Demanze 

## Outils et technologies 

### Technologies
Pour la réalisation de ce projet nous avons décidé d'utiliser du C# couplé au framework .NET de Microsoft. Nous avons choisi ces technologies car nous avions déjà travaillé avec. 
 
### Outils
 
#### Environnement de développement
Nous avons utilisé l'environnement de développement Visual Studio. 
Visual Studio permet de : 
 * Compiler du C#, essentiel pour notre projet. 
 * Réaliser des interfaces à l'aide de l'interface graphique WinForm. Très utile mais uniquement pour la partie statique des interfaces (en général les menus) 
 
 ![editeurFormVS](imageRapport/editeurFormVS.PNG?raw=true)
 
 * Gérer la traduction à l'aide de fichier ressources propre à chaque langage ce qui nous a grandement aidé dans le processus de traduction du jeu
 * Comme pour les IDE conventionnel l'auto-complétion et aussi un mode débogage qui nous a été très utile

####  Versionning
Nous avons utilisé Gitlab pour le versionning, d'une part car nous avions tous déja utilisé Git mais aussi car c'est un outil très efficace. 
Il permet à chaque participant de développer de son côté sans être inquiété des conséquences de potentiels bug à l'aide du système de commit 
qui permet de revenir sur des versions fonctionnelles. On peut aussi facilement identifier la cause des bug en étudiant les commits précedents. 
Git conserve l'historique des commit, on peut donc traquer un bug ou tout simplement s'informer sur les récentes modifications du projet par
les autres membres.

![gitlab](imageRapport/gitlab.PNG?raw=true)

## Conception

Après avoir choisi les différents outils et technologies nous avons pu commencer la phase de conception du projet. Nous avons commencé par réaliser un UML sur google drive pour avoir une vue d'ensemble de notre projet, 
l'UML est très concis car de toute manière il y allait avoir des modifications au fur et a mesure du développement. 
Il a surtout été réalisé pour nous aider à créer le noyau de notre application.

* L'UML original

 ![OriginalMCD](imageRapport/OriginalMCD.png?raw=true)

* Voici l'UML réduit final de l'application 

 ![reducedMCD](imageRapport/reducedMCD.png?raw=true)

* Voici l'UML étendue final de l'application

 ![ExtendedMCD](imageRapport/ExtendedMCD.png?raw=true)

 
 On remarque que les différences sont surtout aux niveau de la vue ou il y a nettement plus d'interfaces, ceci est surtout expliqué par les contraintes du langages qui nous a fait utiliser un
 nombre de formulaires considérable. Néanmoins la couche métier et controlleur sont sensiblement les mêmes ( à l'exception de l'IA) entre l'UML original et l'UML final. L'UML original nous aura
 donné une bonne base pour développer l'application, ensuite nous avons développer les modules (IA , Réseau) qui n'étaient pas prévus sur l'UML original.
 Nous avons utilisé un modèle MVC classique: séparation des objets, de la vue et de la logique globale. 
 
 * La vue permet d'**informer** le joueur sur l'état de la partie mais aussi de **communiquer avec la couche métier** en passant par le controlleur.
 * Le controlleur fait le lien entre la vue et le métier, tout les **controles** sont effectués dans le controlleur (règles de jeu, effet des cartes)
 * Le métier représente les **objets du jeu** et leur **états** a un moment donné.
 
 La vue est dépendante du métier mais pas l'inverse , le jeu pourrait très bien tourner sans vue avec une simple interface console ( ce que nous avons fait au début pour tester le métier).
 
### Répartition du travail

* _IHM & Animations_: 			Jules, Jorick
* _Noyau_: 						Jorick, François
* _Réseau_: 					François
* _IA_: 						Paul, Adrien
* _Internationalisation_ : 		Jules, Adrien
* _Contrôleur_ : François, Jules
* _Débogage & optimisations_ :	Paul, Jorick, Jules


## Implémentation et développement


### Couche métier

La couche métier est la première couche que nous avons implémenté .  
Les objets de base du modèle sont les joueurs et les cartes. 
Nous avons utilisé un design pattern singleton pour l'instance de jeu car l'instance de jeu doit être unique 
et grâce a ce DP l'instance est accesible partout dans l'application. La couche métier est simple néanmoins 
il est crucial de faire très attention lors de l'implémentation, elle est le noyau de l'application et tout problème dans
le noyau peut déteindre sur toutes les autres couches.

### Contrôleur

La couche contrôleur était la seconde phase de conception du noyau après la couche métier. En effet nous avons donc commencé à implémenter les règles du jeu. 
Les règles sont divisées dans 2 classes contrôleur différentes : 
* _Déroulement du jeu_: 	 	GameControler
Début de tour, fin de tour, piocher une carte, jouer une carte...

* _Effets des cartes_: 			CardEffect  
  
Ainsi les contrôleurs ne sont pas surchargés et sont organisés au mieux possible.
De cette façon nous avons également essayé de prévoir au maximum l'ajout des modules "Réseau" et "IA", ainsi il fallait donc implémenter des contrôleurs facilement utilisables et les plus indépendants possible.

### Fin du noyau et test console

Afin de tester la validité de notre noyau du jeu, nous avons rapidement implémenté un affichage console.

 ![ConsoleDebug](imageRapport/consoleMode.JPG?raw=true)
 

### Vue

Nous avons développé les menus à l'aide de l'éditeur d'interfarce WinForm. Cette interface permet de développer des *formulaires* de manière très rapide,
une liste de composants est disponible. Chaque composants à un certains nombre de propriété ediatble directement depuis l'interface, il est possible d'assmilier 
des évenments à un composant ( click, mouseEnter).

 ![winForm](imageRapport/winForm.png?raw=true)
 
L'éditeur a pleinement rempli sa tâche et nous a aidé à développer les menus *statiques*. Statiques en effet car si l'on veut mettre en place des deplacement/positionnement/tailles
dynamique il faut le faire de manière fonctionnel, notamment pour le plateau de jeu.

Nous avons suivi notre maquette pour le plateau de jeu et comme nous voulions une interface qui s'adapte à la taille de l'écran de l'utilisateur nous avons du créer nos composants et les placer
de manière dynamique via le constructeur de notre formulaire de jeu. Le développement de la vue était relativement compliqué notamment à cause des positions des images qui ne cessent de bouger, 
de plus comme je l'ai dit auparavant nous voulions une IHM qui s'adapte à la résolution de l'utilisateur, il faut donc garder ça en tête constamment dès que l'on créer/dimensionne/place un contrôle. 

Nous avons choisit d'utiliser des images pour représenter les cartes, cela rend le jeu plus agréable et facilite la distinction et l'appréhension des différents type de cartes par l'utilisateur.
Cependant à cause du redimensionnement des cartes l'effet inscrit dessus est difficlement lisible.
Nous avons donc opté pour un tooltip lorsque l'utilisateur passe sa souris sur la carte, ce tooltip comprends:

* Nom de la carte
* Valeur
* Effet

 ![tooltip](imageRapport/tooltip.png?raw=true)
 
 Bien entendu le tooltip est traduit.
 
#### Généralités & squelette

Nous avons tout d'abord développé le menu principal. En même temps nous avons décidé de la charte graphique de l'application (police noire, Times New Roman, couleurs sobres marron/brun et quelques touches de rouge, ambiance "anciennes lettres" en fond)
Il a été décidé que les menus seraient non-redimensionnables commme pour un launcher classique de jeu. Ils sont également sans bordures et sans controlbox, en effet il n'est pas necessaire de mettre les menus en plein écran comme il le serait
pour l'interface de jeu. A noter qu'ils sont tout à fait déplaceable au seins de l'environnement Windows et ceci malgré l'absence de controlbox et bordures. L'ergonomie est donc conservée au maximum. 

![MenuPrincipal](imageRapport/MenuPrincipal.PNG?raw=true)

Une fois que le menu principal était développé nous nous sommes attaqués aux autres formulaires (paramètres de jeu / paramètres globaux) toujours en respectant la charte graphique, nous 
voulions que les formulaires restents simples, sobres ainsi que dans le thème du jeu pour que l'expérience utilisateur soit plaisante et immersive.

Pour ce qui est de l'interface de jeu nous avons d'abord dessiné une maquette pour nous donner une idée de l'interface.
Il était clair dès le début, que nous voulions faire une interface en plein écran afin d'être sûr de la lisibilité des différents objets.

![maquetteIHM](imageRapport/maquetteIHM.png?raw=true)

Nous voulions une interface centrée sur l'utilisateur, qu'il joue dans le mode de jeu "Solo" ou "Multi" mais tout en gardant à l'esprit une projection d'une vraie table de jeu, c'est pourquoi nous avons choisis de positionner les cartes en fonction de son joueur,
et non pas toutes les cartes dans le sens de l'utilisateur.
Nous souhaitions une interface réaliste et immersive, nous avons donc essayé de ne pas la surcharger et de n'afficher, dans la mesure du possible, que des objets qui pourraient être présent dans une partie réelle de "Love Letter".

#### Design

Côté design, nous avons continué à nous référer à la charte graphique, toujours la même ambiance de vieilles lettres, les couleurs restent les mêmes que dans les menus etc...
Nous avons travaillé avec des images de cartes d'un vrai jeu Love Letter trouvé sur Internet pour toujours augmenter l'immersion.
C'est images de cartes se marient parfaitement bien avec le fond qui est également une vraie photo de papier à lettre.
A noter que chaque image a été retravaillée par nos soins sur Photoshop pour obtenir un ensemble de photos avec le même contraste et la même netteté.

![debutJeu](imageRapport/debutJeu.png?raw=true)

#### Ergonomie

* Tutoriel

Le tutoriel permet aux utilisateurs non initiés d'apprendre à jouer.

* Adaptabilité de l'interface

L'interface s'adapte à l'écran de l'utilisateur de sorte que l'utilisateur puisse bien voir le plateau de jeu avec
tout ses éléments.

* Animations

Améliore l'expérience utilisateur en rendant le jeu plus intuitif.

* Charte graphique/images

La charte graphique est respecté dans tout les formulaires, de ce fait l'application est cohérente et l'utilisateur
n'est pas dérangé par une interface en particulier. Les cartes sont représentées sous forme d'images pour mieux identifier
les type de cartes ( et c'est joli ). 

* Information sur l'état du jeu / paramétrages

L'utilisateur reçoit des messages lui indiquant les actions du jeu , de ce fait il a pleinement connaissance de l'état 
du jeu et des actions entreprises. Si les messages le dérangent il peut les désactiver via un menu simple d'accès et 
choisir une boite de message moins intrusive.
Les tooltip sur les cartes permettent à l'utilisateur d'avoir pleinement l'information sur une carte (nom/effet/valeur).
De plus les cartes défaussé etant visible (même partiellemnt) l'utilisateur peut savoir ce qui à été joué , la message 
box permet aussi de retracer l'historique des actions des joueurs. Cela permet à l'utilisateur de comprendre ce qui 
se passe sans être dépassé par le jeu.

#### Animations

Nous voulions dans un 1er temps déplacer les cartes d'une position A à une position B de manière à ce que l'utilisateur puisse voir le deplacement de la carte. Malheureusement il 
fallait rafraîchir le formulaire et tout les composants liés au formulaire pour mettre a jour l'affichage de la position de la carte à un instant T. Cela était beaucoup trop lourd
et réduisant considérablement le déplacement de la carte. Nous avons donc décidé de déplacer les cartes sans mouvement de la position A à la position B. 
Ayant du faire ce sacrifice nous avons implémenté des animations pour rendre l'expérience utilisateur plus intituif, par exemple lorsque l'utilisateur 
passe le curseur sur une carte jouable la carte sera agrandie tant que le curseur soit sur la carte. Ce genre d'animation rend le jeu plus *vivant* 
et donc compense la perte du mouvement des cartes.

![animationResizeCard](imageRapport/animationResizeCard.gif?raw=true)


#### Tutoriel

Le mini tutoriel disponible en jeu n'a pas vocation à apprendre les règles du jeu à l'utilisateur.
Ce mini tutoriel donne une indication sur l'action à réaliser lorsque c'est à l'utilisateur de jouer. Il est très facilement activable et désactivable.
> Note : Par défaut il n'est pas activé car il est très intrusif. En effet un joueur connaissant déjà les actions réaliser perdra en plaisir de jeu si il se fait importuner de façon constante par ce mini tutoriel.

![joueUneCarteTuto](imageRapport/joueUneCarteTuto.png?raw=true)

Le design est le même que pour toutes les autres indications pendant le jeu, le fond du jeu est grisé, un texte blanc donne une information et éventuellement les objets en question apparaissent avec un contraste plus élevé.

#### Modules d'affichages d'informations

Nous avons laissé à l'utilisateur d'utiliser 2 types d'affichages pour les messages d'information :

* Messages d'information en plein écran

Notre première option ( qui était la seule à la base) était d'afficher les messages d'information sur un **fond sombre** en **plein écran**, de
cette manière l'utilisateur peut se concentrer sur le message (et surtout ne pas le rater !).

![messageFullscreen](imageRapport/messageFullscreen.png?raw=true)

Cependant l'utilisateur voudrait peut être visualiser le plateau en permanance sans être importuné par les messages 
mais toujours en ayant l'information de ce qu'il se passe. La 2ème option permet de résoudre ce problème

---

La 2ème option disponible pour l'affichage des messages est une boîte de dialogue non interactive (on peut uniquement scroller pour parcourir les messages mais 
on ne peut pas écrire dedans) qui affiche l'ensemble des actions qui se sont passées dans la partie à l'exception du début et de la fin de la partie ou nous
avons jugé les messages en plein écran nécessaires.

* Boite de messages contenant les messages d'informations 

![boiteMessages](imageRapport/boiteMessages.png?raw=true)


---

Bien entendu il est possible d'activer les2 options en même temps , une des deux ou aucunes. La boite de dialogue et les messages en plein
écran **s'adaptent a la résolution de l'écran de l'utilisateur**.


#### Module de sons

* **Généralités**

Nous avons toujours cherché à améliorer l'expérience utilisateur et à faire le maximum pour que notre jeu soit plaisant à jouer.
Dans cette optique nous avons implémenté une ambiance sonore.
Trois types de sons peuvent être joué : 
* _Musique d'ambiance_: 	 	Elle est jouée en boucle à partir du lancement du jeu.
* _Bruitage "jouer une carte"_: Trouvé sur une banque de sons gratuits, c'est un vrai bruitage d'une carte qui est distribuée ou jouée.
* _Bruitage "choisir une carte"_: Également trouvé sur la même banque de sons, c'est un bruitage de main qui tape sur une table. Ce bruitage est utilisé lorsque l'utilisateur clique sur des boutons etc...			


---


* **Détails techniques et moteur de sons**

Nous avons d'abord essayé d'utiliser un module de son très simpliste disponible dans le framework .NET de Visual Studio.
Ce module était fonctionnel mais il était trop basique puisqu'il ne permettait **pas de gérer plusieurs sons à la fois**.
En effet alors que la musique d'ambiance était jouée en arrière plan, et qu'un bruitage intervenait, la musique d'ambiance se coupait.
Il n'était donc pas possible de jouer plusieurs sons en parallèles malgré le placement des objets "Media" dans différents Threads.
De plus cet objet "Media" ne permet que très peu de paramétrage, le volume indépendant des sons n'était pas réglable.

Il a donc fallu utiliser **un module plus puissant**, un module de son "Media Player" **provenant directement du noyau Windows**.
C'est un module tout à fait utilisable en C# mais qui est plus complexe à mettre en place car c'est du code intervenant directement dans les messages internes de Windows.
**Ce module "Media Player" permet donc la parralélisation et paramétrage des sons.**

Alors que le module simpliste "Media" du framework .NET permettait de charger des sons provenant des **ressources intégrées à l'application (.exe)**,
ce module "Media Player" ne le permettait pas et n'accpetait qu'un chemin absolu vers la ressource.
Bien entendu **pour des ressources intégrées à un .exe, il n'existe pas de chemin d'accès**.

Pour remedier à ce problème, il est absolument nécessaire d'avoir les fichiers de son à part et non intégrés.
Mais toujours dans un soucis d'ergonomie, **nous voulions éviter un "installeur"** pour une si petite application, et permettre une portabilité maximale.
Nous avons donc trouvé une astuce, un "hack" en anglais pour **conserver les musiques dans les ressources intégrées**.
Il suffit, au démarrage de l'application, de **créer à la volée des fichiers temporaires à la racine du dossier d'exécution** de l'application.
Ces fichiers temporaires sont, bien entendu, les fichiers de son (.wav), ainsi nous pouvons obtenir un chemin absolu vers ces fameux fichiers grâce au contexte d'exécution.

Après cette astuce, nous pouvons enfin faire fonctionner notre moteur "Media Player".


---


* **Menu de gestion du son**


Nos efforts ont permis de faire fonctionner un moteur de son évolué et complexe, qui permet, entre autre, de gérer des volumes indépendants pour chaque fichier audio chargé.
Nous avons donc rapidement pris la décision de faire profiter l'utilisateur de cette paramétrage disponible.
Nous avons donc créé un menu de son permettant : 

* _Mettre le mode "muet" ou "mute" en anglais_
* _Gérer précisément grâce à une trackbar (0 à 100) le volume de la musique_
* _Gérer précisément grâce à une trackbar (0 à 100) le volume des effets sonores_
* _Gérer précisément grâce à une trackbar (0 à 100) le volume général du jeu_


![menuSon](imageRapport/menuSon.png?raw=true)



### Module IA

Notre IA possède 3 niveaux de difficulté. 

#### IA facile 

Elle joue toujours sa carte la plus faible. Lorsqu'il y a des choix à faire (joueur à cibler / carte à deviner), elle choisit au hasard. 

Ce hasard est déterminé par la classe Random. D'après la documentation .NET, créer une instance de cette classe sans argument génère un seed dépendant du temps. Ce hasard sera donc identique pour deux décisions jouées au même moment. 

#### IA normale

L'IA normale fait des choix par heuristique. Comme un joueur humain, elle se souvient des cartes qu'elle a vue grâce aux effets spéciaux des cartes. S'il y a des coups sûrs, comme faire défausser la princesse d'un ennemi, elle le fera en priorité. 

Si sa carte a été révelée (par un prêtre/roi) et qu'il ne s'agit pas d'un garde ou d'une princesse, elle va s'en débarrasser en priorité pour éviter de perdre à cause d'un garde.

Si elle ne trouve rien d'intelligent à faire, c'est l'IA facile qui prendra la décision.

#### IA difficile

L'IA difficile commence d'abord comme l'IA normale, elle cherche des coups gagnants basés sur sa connaissance des cartes des adversaires. Cependant, si il lui manque des connaissances, elle commence à faire des statistiques pour évaluer les cartes probablement possédées par les adversaires. Pour cela, elle mémorise en plus les cartes qui ont été défaussées. Comme elle sait combien de cartes il y a en jeu, elle peut déterminer une probabilité qu'un adversaire possède une carte en particulier. 

![Shéma Switch](imageRapport/ia_1.png?raw=true)

Elle peut donc faire les même choix que l'IA normale (utiliser un prince pour faire défausser une princesse) mais cette fois sans certitude, seulement avec une probabilité haute que l'ennemi possède réellement la carte. 

Pour arriver à faire ces statistiques de manière précise, l'IA doit prendre en compte:
- Les cartes défaussées.
- Les cartes qui lui sont actuellement révelées (par un prêtre/prince). 
- Les cartes qui ne peuvent pas être possédées par l'adversaire (quelqu'un a essayé de deviner et a échoué). 

Ce comportement est difficile à adopter pour un humain car il faut beaucoup de mémoire et de déduction. Pour une machine, c'est extrêmement facile. Cette stratégie est très efficace vers la fin de la partie car les cartes restantes deviennent de plus en plus facile à deviner. 

***

### Réseau

Avant de commencer, il est important de noter que la partie réseau ne fonctionne pas sur les ordinateurs en salles machines. En effet nous avons fait le choix d'utiliser une machine serveur ouverte sur un port spécifique. 
Hors n'ayant pas les droits d'administrations sur les systèmes Windows il n'est malheureusement pas possible d'ouvrir les ports réseaux pour accéder au serveur.
Nous avons donc commencé à reflechir à plusieurs solutions.

* Utilisation d'ordinateurs portables connectés sur le réseau Eduroam: Cette méthode ne fonctionne pas car l'ensemble des ports est bloqué. Nous avons tenté de contacter le support mais aucune ouverture nous a été autorisé.
* Utilisation d'ordinateurs portables connectés sur un réseau partagé 4G mobile: cette méthode ne fonctionne pas non plus, car beaucoup de ports sont bloqués par les opérateurs. L'utilisation d'un VPN aurait pu régler le problème, mais nous ne voulions pas utiliser trop de tiers services.
* Utilisation d'un câble Ethernet pour connecter 2 ordinateurs portables: cette méthode fonctionne mais présente 2 inconvénients majeurs: l'utilisation d'un câble Ethernet croisé (plus rare et cher qu'un câble Ethernet droit) et le fait qu'on peut uniquement connecter 2 machines.
* Utilisations d'un commutateur réseau (switch) pour connecter plusieurs ordinateurs entre eux: cette méthode est fonctionnelle, et prouve le bon fonctionnement du module réseau malgré que c'est une topologie LAN.

Nous avons donc choisis d'utiliser un commutateur réseau pour démontrer les capacités du module réseau.
Ainsi notre application réseau peut s'utiliser comme un module permettant de jouer en ligne mais également en LAN selon les configurations.

> Note: Un switch est un équipement réseau permettant de connecter plusieurs machines entres elles de la facon suivante:

![Shéma Switch](imageRapport/switch.PNG?raw=true)

Comme annoncé plus haut, nous avons fait le choix d'utiliser une machine serveur, ce qui permet à n'importe quel utilisateur de pouvoir créer ou rejoindre une partie de LoveLetter mais sans devoir l'héberger.
En effet le joueur peut créer un lobby, mais ne créé pas de serveur à proprement parler, il n'héberge rien. Cela évite les problèmes de configurations pour l'utilisateur.
Le serveur est un projet qui se build à part, mais qui contient les mêmes classes que le projet Love Letter (hormis les vues car c'est un projet uniquement en affichage console).
  
> Note : Du fait que les utilisateurs se connectent à un serveur dédié et non pas à un serveur de leurs création, la tricherie est plus compliquée (pas impossible) puisque les données sont traitées par un tier de confiance neutre.


Le protocole choisis est le protocole réseau UDP contrairement au protocole TCP le protocole UDP ne garantit pas la livraison du message. Néanmois les méthodes 
de communication choisis permettent une connexion fiable. Nous avons fait le choix de l'UDP car il est plus rapide et plus simple à mettre en place.
  
> Note : Un mode de fonctionnement en TCP aurait également été fonctionnel.

Le serveur et le client communiquent grâce à une classe Message qui permet d'echanger des informations dans un format particulier:

Un objet Message a une taille maximale de 1500 Octets:

Les attributs sont les suivants:
* Commande: cet attribut permet aux serveurs ou clients de comprendre comment traiter ce message par exemple(Commande.CREATELOBBY = creation de partie, Commande.PLAYPRINCE = un joueur joue un prince)
* CommandeType: peut prendre 2 valeurs: Requête/Réponse qui permet un dialogue entre le client/serveur
* Data: données transmises dans l'objet message, cela peut être un message textuel destiné à un tchat ou des données permettant la création d'une partie
* DataSize: Permet de délimiter la longueur du champ Data afin de limiter la taille de l'objet
* Sender: Nom de l'envoyeur

> Note: Le champ data permet de contenir plusieurs informations distinctes qui seront stockés dans un seul attribut. Par exemple si un joueur veut créer une partie il devra lui donner un nom et un nombre de joueur max. 
Ceci sera stocké de la manière suivante dans un string : "NOM_PARTIE|JOUEUR_MAX"
Lors de l'arrivé du message il suffira d'appliquer une méthode String.Split('|') afin de récupérer les 2 informations.
Cette méthode permet d'économiser des attributs dans l'objet Message.

Des méthodes de sérialisation et de déserialisation permettent la récuperation du bon message entre le client et le serveur.

Un objet client fait également son apparition, il est relié à un joueur et possède une socket pour envoyer et recevoir un message sur un serveur.

A partir d'ici tout devient possible.

Le module réseau se décompose en 2 parties distinctes:

#### Gestion des lobbys

Lorsque le joueur se connecte au serveur il aura accès à l'ensemble des parties disponibles. Il pourra soit rejoindre une partie existante, ou en ajouter une nouvelle.
Un tchat est également disponible afin de communiquer avec les autres joueurs directement sur le serveur ou alors dans le lobby en pré-game.


Une fois une partie rejointe il pourra accéder à la liste des joueurs connectés et également communiquer avec eux. Une fois tout les joueurs presents, la partie pouura commencée.

Lorsqu'un joueur creer/rejoins/quitte une partie, l'interface se rafraichira automatiquement avec les nouvelles informations envoyées par le serveur, ce qui permet une information constante.


> Note: Lors de la création d'un lobby le serveur va ouvrir une connexion sur un nouveau port unique, 
il enverra ensuite ce numéro de port au client voulant rejoindre ce lobby ce qui permet d'isoler les différents lobbys entre eux.

> Note: L'attente des messages étant une méthode bloquante, les messages serveurs sont attendus dans un thread, qui lui gère l'arrivé des messages et mettra l'IHM à jour. 

Menu du Lobby            |  Menu avant début partie 
:-------------------------:|:-------------------------:
![Lobby](imageRapport/Lobby.JPG?raw=true)  |  ![newgameFrançais](imageRapport/BeforeGame.PNG?raw=true)

Cette partie est entièrement opérationneles, et ne comporte aucun bug connu.

#### Gestion du jeu : BETA

Une fois que tout les joueurs ont rejoint une partie, le jeu se lance. La partie est gérée du coté serveur. Le serveur mélange les cartes et choisit qui va jouer en premier.
Ils envoient ces différentes informations à tout les clients connectés, ils vont ainsi mettre leur plateau de jeu à jour.

> Note: Le plateau de jeu est exactement le même que celui utilisé pour la partie Solo

Lors ce qu'un joueur pioche/joue une carte, il envoie cette information au serveur qui va la stocker, et envoyer un message à tout les autres clients pour informer quel joueur a fait quelle action.

Tout les effets sont fonctionnels et s'affichent de la même façon que dans le cadre d'un jeu en "Solo". L'information est envoyée au serveur qui, à son tour, dispatche aux autres clients.

Lors ce qu'une partie se termine, tout les joueurs retournent au menu.

Si un joueur quitte une partie en cours, tout les joueurs quittent la partie et retournent au menu.

Nous avons, dans la mesure, tester la robustesse des parties multijoueurs. Dans 90% des cas, environ, le jeu se déroule sans bugs. 
Cette partie comporte encore quelques légers bugs qui peuvent, dans le pire des cas, causer la perte d'un client donc l'arrêt de la partie en cours.
C'est donc du fait que les parties multijoueurs ne sont pas à 100% stables que nous avons choisi de mettre le mot clef "BETA". 

#### Test réseaux

Afin de tester le module réseau nous avons utilisé WireShark qui est un programme analysant les paquets circulants sur le réseau. Ainsi nous avons pu vérifier si les paquets
envoyés/reçus étaient corrects.

Néanmoins par manque de moyen la partie réseau ne fut pas tester à grande échelle. Nous ne savons pas comment le serveur ce comporterait en cas de connexion massive de joueurs, ou lorsque plusieurs parties sont jouées en même temps.
On peut néanmoins affirmer, avec plus ou moins de certitude, que dans le cas d'un serveur utilisé dans une topologie de LAN, le serveur est robuste.

#### Difficultés réseaux

De nombreuses difficultés ont été rencontrés pour ce module:
* Le fait que le module réseau ne fonctionne pas sur les ordinateurs de l'université, ce qui est très décourageant.
* Tester l'application est compliqué car il faut lancer plusieurs clients, ce qui prend du temps pour tester tous les cas possibles. (un script de test n'est pas envisageable)
* La gestion des sockets dans des threads est complexe.
* Difficultés au niveau de la compréhension de la gestion des Threads: quand un thread est lancé dans un formulaire, si on ferme le formulaire le thread continue en arrière-plan, il faut le gérer sinon cela cause des problèmes de concurrence.


### Internationalisation

#### Traduction

Le jeu a été entièrement traduit en français, en anglais et en allemand.

#### Localisation

Quand on change la locale de l'ordinateur, si on la met en anglais français ou allemand, le jeu démarre automatiquement dans cettte langue et toutes les interfaces et les messages seront dans la
langue de l'ordinateur. Bien entendu il est possible de changer la langue de l'application pour anglais/français/allemand.

#### Internationalisation

Il n'y a pas de concaténation de chaînes de caractères dans le code et encore moins de texte écrits en dur, tout est dans des fichiers .resx qui sont les fichiers d'internationalisation de .NET.
Après la création d'un formulaire, il est possible de recréer ce même formulaire pour chaque langue et biensur d'adapter le texte et le positionnement/tailles des contrôles en fonction de la langue.

Menu de jeu allemand             |  Menu de jeu français
:-------------------------:|:-------------------------:
![newgameAllemand](imageRapport/newgameAllemand.PNG?raw=true)  |  ![newgameFrançais](imageRapport/newgameFrançais.PNG?raw=true)

Comme on peut le voir ci dessus , on le même formulaire mais adapté pour 2 langues différentes, cette fonction est un peu lourde car il faut generer un formulaire par langue néanmoins
elle permet d'adapter de manière pratique une interface pour un pays.

Comment traduire les messages / indications à l'écran qui sont dynamiques ?
.NET propose des fichiers ressources qui eux aussi peuvent être propres à chaque langue (ou alors culture selon les termes .NET.

Fichier de ressources allemand             |  Fichier de ressources français
:-------------------------:|:-------------------------:
![resxAllemand](imageRapport/resxAllemand.PNG?raw=true)  |  ![resxFrançais](imageRapport/resxFrançais.PNG?raw=true)

Ci dessus on peut voir chaque String associé à un nom (une clé en quelque sorte), or lorsque nous voulons afficher un message 
à l'écran nous allons faire appel à la string associée au nom donné en paramètre : 

```csharp
	GameController.gameView.showMessage(String.Format(Properties._string.NewRound, Game.Instance.roundNumber),true);
```

Dans ce bout de code on veut afficher un message pour indiquer le nouveau round avec comme paramètre le numéro de round.
On fait donc appel à notre gestionnaire de ressource avec la clé de la String correspondante et le numéro du round en paramètre de la String formatée :

```csharp
	Properties._string.NewRound, Game.Instance.roundNumber);
```

La String retourné dépendra de la culture courante (langue courante).
Ce système de fichier ressources est très pratique néanmoins il faut faire attention lorsque l'on ajoute une nouvelle string de bien traduire directement dans tout les langues 
de l'application ou au moins d'ajouter un commentaire indiquant dans quelles langues il faut traduire la String. Les fichiers peuvent devenir rapidement chargés et on oublie rapidement 
de traduire une String ajouté de manière précipité.

## Difficultés techniques rencontrées et solutions

### Problèmes liés aux outils utilisés

#### Problèmes de framwork et language

Comme indiqué plus haut, nous voulions au départ animer l'application avec des déplacements de cartes et autres modifications dynamiques. Mais nous avons rapidement constaté que Windows Forms n'est pas approprié pour ce type d'application: ce framework semble plutôt conçue pour des formulaires statiques. 

Nous modifions tout le même le formulaire au fur et à mesure de la partie avec l'apparition/disparition de cartes ou de messages. Cela crée des latences assez désagréables pour l'utilisateur car le formulaire prend du temps à s'adapter.

Une partie de ces latences peut également être lié à C#, qui fonctionne avec un garbage collector et nous pousse donc à produire des ressources dynamiquement sans gérer le nettoyage.

## Tests

Les tests ont été effectués tout d'abord grâce à une console pour tester le noyau du jeu. Les autres tests ont étés effectué en
jouant en jeu mais aussi en testant le cas par cas en modifiant le code de manière temporaire. Lorsqu'un comportement anormal était
detecté on utilisait le debugger pour traquer et corriger le problème

## Résultat final et conclusion

Finalement notre application est séparée en *3 couches* (MVC) et propose une implémentation du jeu Love Letter suivant les règles du jeu original avec les modules IA et réseau.

Nous avons vraiment voulu que l'utilisateur retrouve *l'expérience d'un plateau de jeu standard* , ce qui impliquait les images pour les cartes, 
les joueurs aux 4 coins de la "table". Cela a impliqué de nombreuses contraintes mais nous sommes satisfait du résultat final. De plus la charte graphique
apporte une ambiance qui correspond vraiment à l'univers du jeu. 

Malheureusement il y a eu certains problèmes que nous avons du contourner comme le déplacement des cartes qui n'était pas fluide du au framework/langage.

**L'outil de débogage** a été très utilisé, c'est un **outil indispensable** si l'on ne veut pas perdre de temps pour la résolution d'un bug. L'outil inclut dans
visual studio est très puissant et nous a économisé des jours de travail. Pour ce qui est du **versionning Git** est aussi un outil qui nous a grandement aidé,
il est facile de **tracer l'historique des modifications** à l'aide du système de commit et donc d'observer les modifications des autres développeurs mais aussi de **traquer 
les potentiels commit** qui auraient provoqué **un ou des bugs**.

Un projet de ce type ou l'IHM est important et ou le nombre de personnes dans l'équipe permet d'en apprendre beaucoup sur le travail d'équipe, le versionning et les 
subtilité de l'IHM, de la conception et du développement.
Bien répartir les tâches , ne pas hésiter à aider les autres membres même si l'on ne travail pas sur la même fonctionnalité sont les points clés pour réussir un projet dans les temps.
Bien commenter les commits et ne pas hésiter à commit régulièrement,  il est préférable d'avoir plusieurs commit léger bien commenté qu'un seul commit qui modifie un trop grand 
nombre de fichiers et qui nécessite un pavé en guise de commentaire.

Il est aussi important de se mettre à la place de l'utilisateur et plus particulièrement en fin de développement la ou il est le plus difficile de sortir du code et 
de faire comme si c'était la première fois que nous lancions l'application.

Ce genre de projet permet aussi de s'intéresser au fonctionnement profond d'un langage ou d'un framework, notamment au niveau de l'IHM. Savoir comment et quand est dessiné/redessiné un contrôle,
comment est gérer la mémoire. Ce genre de questions doit être posé si jamais il est questions d'optimisation.
Avoir un code générique pour faciliter la réutilisation mais pas trop car cela peut prendre trop de temps est essentiel. Il faut penser aux personnes qui devront débugger, réorganiser 
le code, les commentaires sont aussi importants que l'organisation du code.

---

Nous avons beaucoup appris tant bien au niveau technique que organisationnel, ce genre de projet constitue un challenge et peut parfois prendre les nerfs lorsque l'on arrive pas 
à résoudre un certain problème. Il y a toujours une ou plusieurs solution , la question est de savoir quelles seront les conséquences de l'implémentation de cette solution.




