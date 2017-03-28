

-------------------------------------------------------------------------------------------------------
The main features added to the classic pong game will be:
Being able to play against other human over the network (local network or over the internet)
We want to be able to remotely change the assets for the game (for example during christmas the background will be a snowy field, the paddles will be santa claus and it will feel icy, during halloween the paddles will be bats and the ball will suddenly scare the player by shouting and leaving blood behind…) we would like to have as much freedom as possible to change the assets.
There will be no IA moving the “enemy” paddle, if there is no enemy the player will have to wait.

-------------------------------------------------------------------------------------------------------
Development procedure

One player game against a wall, no enemy paddle.
Two player game using the same computer, no networking
Two player game over the network
Asset customization system.
Please tag each “release” in the repo for us to keep track of it.
-------------------------------------------------------------------------------------------------------

Design tasks:

One player game against a wall, no enemy paddle.
- Create collision arena
- Create paddle/bat with collision
- Create ball with collision 
- Add user input to control Bat
- Add score for successful hit
- Test Ball<->Arena Ball<->Bat and Score

Two player game using the same computer, no networking
- Expand arena - collision as well
- Create second player paddle/bat with collision 
- Add second user input 
- Add second score for successful hit
- Test Both Bats<->Ball and Score

Two player game over the network
- At startup determine Client or Server (user chooses by selecting "Host or Connect")
- Server
- Add server control for ball and bats (entities)
- Wait server until client connect (GUI notify user)
- On Client connect - countdown start.
- Client
- Enter server IP or domain name
- Connect with server
- Sync Ball and Bats with Client/Server raknet components
- Test All 

Asset customization system.
- At startup check for new assets (looks for specific site with txt file - using github to make it easy)
- Found assets then download list of asset elements (Bats, Balls, Score Text, Background)
- Start Game (waiting for client).
- If Client - check server for new assets before game countdown. 
-- Download to client if notified.
- Start Coutdown and begin game

-------------------------------------------------------------------------------------------------------

Notes:
- Asset customisation should be done as part of initial development. It impacts the use of assets within
    the game and this needs to be consistent. Changing all asset references later would be extra work.
- Input for bats and movement of ball should be abstracted so the network objects can override them later.
- Generally I wouldnt use the builtin Raknet comms. I prefer writing a managed comms handler (for event handling and better data packaging). This would be how I would evolve the networking element. As well as moving to a peer to peer type data communication so that a server hot negotiates the connectivity (ala WebRTC).
- I would also normally use a simple FSM Game Manager. I may do this, but its overkill for the task.

-------------------------------------------------------------------------------------------------------