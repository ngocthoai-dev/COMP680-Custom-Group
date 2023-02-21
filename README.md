# COMP680-Custom-Group
2D multiplayer fighting game of Custom Group for COMP680 assignment. For our project we are proposing a 2D Multiplayer Fighting game akin to the Super Smash Bros. and Street Fighter franchises. Players will be able to connect with friends and brawl it out to see who is the better fighter. There will be a selection of fighters and maps to choose from. The game will support online multiplayer and include a small set of host management tools, such as the functionality to remove unwanted players from a match.

This project used Zenject, dependency injection framework, as a core to manage the scripts.

# Framework
The framework or project architecture will be started from the _Project directory.

    - Bundles: the folder holds every assets that could be managed by addressables or resources such as prefabs, graphics, audio, etc.
    - Scenes: the scene for building game levels separately. The core is start scene. We almost alwayswork on prefabs -> avoid conflict issues on the scene.
    - Scripts:
        - Business: a project that contains almost interface that can be exposed to any projects.
        - Core:
            - Signal: signal container for observer pattern, which is implemented by Zenject SignalBus.
            - Game: every specific game login will be run from here.
            - Services: it holds services running through the game for Core project such as network.
            - Modules: this contains the mediators or the controllers for their own view implementation. 
            - Views: the views scripts config for corresponding assets will be stored here.
        - Editor: a project customize unity editor.
        - Extensions: a project give a extendiable manipulation based on object.

The behavior after initialized project will be runned by GameStore Initialize method, which will immediately run into StartSessionScreenController.