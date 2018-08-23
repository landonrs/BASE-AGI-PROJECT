# BASE-AGI-PROJECT

A software robot that learns about objects and entities in its environment.

The Base Artificial General Intelligence project is a experimental attempt to create an Agent that is able to develop a general understanding of its environment by interacting with objects and storing meta data about those objects within an internal Database.
The Agent then uses this data to respond to Natural language commands. The Database is structured in such a way that the Agent is able
to determine what words define objects as opposed to adjectives, verbs etc.

This readme outlines the various components that are used in this project and how they can be developed upon. This project is developed using the Unity Engine for the purpose of handling the physics and navigation so the main focus of development can be placed on improving the Agent's ability to respond to commands. 

## Classes
This section outlines the classes for this project. This classes are all written in the C# language in order to be compatible with the Unity Engine. 

### Database Classes
These classes interact with the DB to store metadata about the environment.

#### NeoMemory
This class is responsible for creating the internal connections between all the metadata when the Agent interacts with the environment (ie what color the object is, where it is located. etc.) 

#### WernickeArea
This class parses natural language queries and determines which commands need to be executed based on the information that is provided in the query. It then makes the necessary calls to make those commmands happen.

#### DBUtils
This static class is meant to make it easier to run SQL queries and connect to the project's SQLite DB file. It is recommended to add any methods that involve working with the DB in this class. 

### Scanner Classes
The following classes are responsible for pulling data from the environment and passing it into the Database classes for storage.

#### LocationScanner
Whenever the Agent walks into another room in the house, this class pulls data from the LocationData class and updates the DB with the metadata about the location.

#### ObjectScanner
Whenever the Agent touches an object, this class pulls the data from the ObjectData class for that object and passes that information into the NeoMemory class to be stored in the DB. If the Agent has already interacted with the object before then it is ignored by the DB.

### Data classes
These classes store the meta data for objects, locations and entities.

#### EntityData
This script should be attached to every character in the environment, including the Agent. It gives the agent info about the character(name, location, etc.)

#### ObjectData
This script should be attached to every object that is in the environment. It gives the Agent info about various attributes of the object(color, name, category, etc.)
