# BASE-AGI-PROJECT

A software robot that learns about objects and entities in its environment.

The Base Artificial General Intelligence project is a experimental attempt to create an Agent that is able to develop a general understanding of its environment by interacting with objects and storing meta data about those objects within an internal Database.
The Agent then uses this data to respond to Natural language commands. The Database is structured in such a way that the Agent is able
to determine what words define objects as opposed to adjectives, verbs etc.

This readme outlines the various components that are used in this project and how they can be developed upon. This project is developed using the Unity Engine for the purpose of handling the physics and navigation so the main focus of development can be placed on improving the Agent's ability to respond to commands. 

## Running the program
This project is built using the Unity Engine. To run this program you must install [Unity from their website](https://unity3d.com/get-unity/download). Once installed you can clone this repo onto your local machine and then open the folder you cloned this repo into in Unity. Once the project is opened select the play button that is located near the center top of your view. This will start the program.

## Using the program
Once the program is running, you control the observer that is able to move around the house and interact with the Agent(the green cube). you can control your character using the WASD keys and look around using your mouse/touchpad. You can interact with the Agent using the text field that appears on the bottom half of your screen. As of version 1, the Agent responds to the following statements:
### What Queries: you can ask it what color the objects in the house are (ie what color is the tv)
### Where Queries: you can ask it where objects are located (ie where is the refrigerator) You can also ask it what room you are standing in with the phrase "where am I" or where it is currently in by asking "where are you".
### "Go to" commands: You can tell it to go to a room or towards an object (ie go to the kitchen)

You can ask the Agent about any of the objects in the house, as of version 1 those objects are:
<ul>
  <li>bed</li>
  <li>nightstand</li>
  <li>couch</li>
  <li>tv</li>
  <li>oven</li>
  <li>refrigerator</li>
  </ul>

and the three rooms in the house are:
<ul>
<li>kitchen</li>
<li>main room</li>
<li>bedroom</li>
</ul>
In order to get a response from the Agent, any query you make about an object must be spelt correctly*(Note that the project does not currently handle punctuation)*. If the object name you type in does not match anything that the Agent has interacted with it will return nothing. If the Agent is able to make a match it will output the answer through the Unity Console.

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
This script should be attached to every character in the environment, including the Agent and the observer. It gives the agent info about each character(name, location, etc.)

#### ObjectData
This script should be attached to every object that is in the environment. It gives the Agent info about various attributes of the object(color, name, category, etc.)

#### LocationData
This script must be attached to every room boundary object in the house. It lets the agent know where its location is so when it interacts with objects, it is able to remember the location that it found the object in.

### Unit Tests classes
Unit test classes have been added for several of the classes in order to quickly validate that the code functions as intended. These classes rely on mock DB files that contain a copy of the actual DB schema. If any modifications are made to the schema, it will need to be reflected in the test DBs as well.


