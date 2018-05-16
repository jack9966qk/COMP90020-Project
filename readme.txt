# COMP90020 Project

This project is created with Unity 2018.1.0f2.

In order to build, open the projecy with Unity, go to `File - Build settings...`, make either `Client` or `Server` the only scene enabled to build client and server respectively.

Client attempts to connect to server upon launch, and server starts listening to clients upon launch. When the number of connected clients reaches the number of players, the server starts the game.

Player movement is controlled by arrow keys, while bullets can be shot by pressing space key.

Configurations, such as number of players, server address and port number, can be done by modifying the constants in `Constants.cs`.