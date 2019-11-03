# Game <br/>

Avec l'arrivé du multiplayer sur le jeu, il est nécessaire d'avoir un système serveur-clients. Il faudra donc: <br/>
- Lancer le serveur qui permettra de host tous les clients.<br/>
- Lancer un client pour tester la connexion et les fonctionnalités du jeu.<br/>

<br/>

SERVEUR: <br/>
- Lancez l'éxécutable dans "Build - Windows".<br/>
- Cliquez sur "PLAY". <br/>
- En haut à gauche sur le network manager hud, cliquez sur "LAN Server Only(S)".<br/>

<br/>

CLIENT: <br/>
- Copiez / Collez le contenu du dossier "BuildInfo" vers \Front\src\game\Unity\BuildInfo<br/>
- Lancez le serveur REACT avec npm run dev<br/>
- Rendez-vous sur http://localhost:3000/unity ou l'adresse du frontend.<br/>
- Allez dans la section UNITY.<br/>
- Le jeu se lance. Cliquez sur PLAY.<br/>
- Repérez l'écriture "localhost", en haut à gauche sur le network manager hud, à coté de "LAN Client(C)".<br/>
- Si nécessaire: Ecrivez à la place de "localhost" l'adresse IP de l'adresse du frontend.<br/>
- Un haut à gauche sur le network manager hud, cliquez sur "LAN Client (C)"<br/>

