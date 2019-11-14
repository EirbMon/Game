# Game <br/>

Avec l'arrivé du multiplayer sur le jeu, il est nécessaire d'avoir un système serveur-clients. Il faudra donc: <br/>
- Lancer le serveur qui permettra de host tous les clients.<br/>
- Lancer un ou plusieurs clients pour tester la connexion et les fonctionnalités du jeu.<br/>

<br/>

-- EN PRODUCTION : --

SERVEUR : <br/>
- Le serveur à lancé est situé dans le dossier "BuildLinux" du répertoire GitHub/Game.<br/>
- Pour lancer sur linux, placez dans le dossier récemment télécharger avec "cd \le_repertoire\BuildLinux\"
- Ensuite, accordez vous les permissions sur le fichier Eirbmon.x86_64 avec "chmod +x Eirbmon.x86_64" <br/>
- Pour finir, éxecuter le simplement avec "./Eirbmon.x86_64".<br/>
- DevOps: Automatiquement mis à jour dans le NUC via GitHub. <br/>
- DevOps: Lance automatiquement sur l'addresse IP du NUC sur le port 7777<br/>

CLIENT: <br/>
- Le client à lancé est situé dans le dossier "BuildInfo" du répertoire GitHub/Game.<br/>
- DevOps: Automatiquement mis à jour dans le NUC via GitHub. <br/>
- DevOps: Lance automatiquement sur l'addresse IP du NUC sur le port 3000<br/>
- Naviguer sur: http://@ip_nuc:3000/<br/>
- Allez dans la section UNITY via le menu en haut à gauche.<br/>
- Le jeu se lance. Cliquez sur PLAY.<br/>
- Repérez l'écriture "localhost", en haut à gauche sur le network manager hud, à coté de "LAN Client(C)".<br/>
- Ecrivez à la place de "localhost" l'adresse IP de l'adresse du server : http://@ip_nuc:7777/ .<br/>


-- EN LOCAL : --

SERVEUR: <br/>
- Télécharger les fichiers du répertoire GitHub/Game dans "BuildLinux".<br/>
- Lancez l'éxécutable "Eirbmon.exe" dans "Build - Server".<br/>
- Cliquez sur "PLAY". <br/>
- En haut à gauche sur le network manager hud, cliquez sur "LAN Server Only(S)".<br/>
<br/>


CLIENT: <br/>
- Télécharger les fichiers du répertoire GitHub/Game dans "BuildInfo".<br/>
- Télécharger les fichiers du répertoire GitHub/Front tout.<br/>
- Copiez / Collez le contenu du dossier "\Game\Build - Client\" vers \Front\src\game\Unity\BuildInfo<br/>
- Lancez le serveur REACT avec la fonction "npm start" en étant au préalable dans le chemin \Front\ <br/>
- Rendez-vous sur http://localhost:3000/ .<br/>
- Allez dans la section UNITY via le menu en haut à gauche.<br/>
- Le jeu se lance. Cliquez sur PLAY.<br/>
- Repérez l'écriture "localhost", en haut à gauche sur le network manager hud, à coté de "LAN Client(C)".<br/>
- Un haut à gauche, cliquez sur "LAN Client (C)"<br/>

/!\ Le port par défaut du serveur est 7777 /!\

