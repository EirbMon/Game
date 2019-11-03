# Game

Avec l'arrivé du multiplayer sur le jeu, il est nécessaire d'avoir un système serveur-clients.


SERVEUR: 
• Lancez l'éxécutable dans "Build - Windows".
• Cliquez sur "PLAY"
• En haut à gauche sur le network manager hud, cliquez sur "LAN Server Only(S)"

CLIENT:
• Copiez / Collez le contenu du dossier "BuildInfo" vers \Front\src\game\Unity\BuildInfo
• Lancez le serveur REACT avec npm run dev
• Rendez-vous sur http://localhost:3000/unity ou l'adresse du frontend.
• Allez dans la section UNITY.
• Le jeu se lance. Cliquez sur PLAY.
• Repérez l'écriture "localhost", en haut à gauche sur le network manager hud, à coté de "LAN Client(C)".
• Si nécessaire: Ecrivez à la place de "localhost" l'adresse IP de l'adresse du frontend.
• En haut à gauche sur le network manager hud, cliquez sur "LAN Client (C)"

