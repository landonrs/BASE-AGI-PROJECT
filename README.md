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

## Database Schema

As of this writing, the Agent’s experience with the environment is stored in a SQLITE relational database.
The database file is stored internally in the project under the filename “neo_test.db”. 
This section describes the different tables in the DB and their main purpose.

![alt text](https://github.com/landonrs/BASE-AGI-PROJECT/tree/master/Assets/Documentation/NEO_DB_ERD_v3.png)

### ADJECTIVES – the adjectives tables holds descriptive words of the English language (red, heavy, soft etc.). It is linked to the attributes table through the ADJECTIVE_TYPE table as explained below.

### ATTRIBUTES –the attributes table holds words that define different aspects of objects (color, weight, texture, etc.). These are linked to adjectives in order to define what type of adjective we want the agent to search for in order to find the correct information. 
(ex. If we want to know which objects are red, the agent will need to search the ‘color’ aspect of each object because the adjective ‘red’ is a type of color.)

### CATEGORIES – this table holds categorical words (food, weapon, money). This table is important for categorizing similar objects together (ex. apple and banana are both types of the category fruit). An object can have many categories and a category can belong to many objects, so we link the objects table and the category table together with an OBJECT_CATEGORIES table.

### COMPARATORS– This table holds comparative words (heavier, lighter, harder, softer, etc.). It holds a column for “greater than” words (heavier, harder) and another column for “less than” words (lighter, softer). It will be used to allow NEO to compare objects together based on a particular attribute. (ex. which objects are heavier than an orange? Heavier is linked to the attribute weight so NEO will search its objects table for objects whose weight value is greater than that of an orange.) This table is linked to the attributes table through the ATTRIBUTE_COMPARATOR table.

### LOCATIONS – this table holds information about locations in NEO’s environment. It holds the location name and an x and y coordinate for the location. This allows NEO to travel to different locations in the environment by forming a path from its current location to the coordinates of the saved location. In the future we hope to implement actual dimensions for each location so NEO can always determine what the name of its current location is.

### OBJECTS – the objects table holds the names of the objects that NEO has interacted with. It is connected to the ATTRIBUTES and CATEGORIES tables through the linking tables OBJECT_DESCRIPTION and OBJECT_CATEGORIES, respectively. This allows an object to have many values for the same attribute (an ‘Australian Shepard’ object can have multiple colors) and belong to multiple categories (the ‘Australian Shepard’ is a type of dog and animal). These linking tables allow us to freely add new attributes in the future without needing to change the database schema.

### QUANTIFIERS – This table holds quantity words such as ‘all’, ‘some’, ‘few’, etc. These can be used to specify the quantity of objects needed for certain tasks. (ex:  Eat ‘all’ of the food)

### VERBS – this table holds the command words that neo is able to perform. It is linked to the categories table through the VERB_CATEGORIES table to determine which verbs can be used with which objects (ex: The verb ‘swim’ could be linked to the category ‘liquid’. If a user says “swim in this water” NEO checks the categories of water and determines it is a liquid, so it can swim in it). 

## Developing Natural Language Commands

This section describes how to take a natural language command and translate it into an executable command using the database schema. Consider if you were to write a command for NEO to pick up an object in a programming language, the function header might look something like this.

**Function PICK_UP (OBJECT object_name)**

Where **PICK_UP** is the name of the function and **object_name** is which object you want NEO to pick up. Now consider you want NEO to pick up the first object it sees with a particular attribute. You might overload the function like this:

**Function PICK_UP(OBJECT object_name, ADJECTIVE adjective_name)**

Where **adjective_name** is the attribute value you are looking for. How can we replicate this function header using natural language? Consider the following sentence:

**_Pick up the green apple_**

This sentence contains all of the information we need to run our function using the specified parameters, OBJECT(apple) and ADJECTIVE(green). We also know which function we need to run through the words ‘pick up’. How do we know which words are the function name and which are the parameter values? The database schema: by accessing the words in the VERBS, ADJECTIVES, and OBJECTS tables, we can determine which parts of the sentence are which and what the user is asking for when they say “(pick up –verb) the (green – adjective) (apple – object)” We iterate through the sentence word for word and find matches in the tables, then return the function that is to be executed along with the necessary variables. 

What is so powerful about using the relational database approach is as long as the proper information is given in a sentence, we can parse it into an executable command and use that command on a wide variety of objects and in a wide variety of situations. We have already seen this in NEO’s ability to group objects together based on their attributes (color, weight). Obviously there must be certain logic checks in place to make sure the object can be used for a specific task (ex. you can’t eat rocks or swim in the sky). But this can be done by categorizing objects and then labeling verbs so they can only be used with certain categories of objects (you can only eat objects labeled as food and can only swim in liquids).

So if you are interested in creating new functions for NEO to perform in its environment, first create a function header for the action. The function name becomes your verb, and your parameters will determine which tables you will need to search in order to find the necessary information to run the function. Then when translating the natural language sentence, simply parse the sentence word for word, searching each table in the DB for the word, and find matches for your function name and for each parameter. 
This method should be used in cases where NEO is performing a physical action in its environment, in the case that you want to query him about relationships between objects, the process is a little different, consider these sentences:

**Which objects are red?**

**What is the color of the couch?**

In these cases, the words ‘which’ and ‘what’ should be considered as special types of verbs, it is a word that signifies the user wants NEO to list objects or attributes of an object. Inquiry words require us to determine what attribute is being asked for and which set of objects we are searching through for this attribute. The ‘objects’ word signifies that we are to simply search the entire table of objects, but what about a sentence written as:

**Which fruit are yellow?**

Now our set of objects to search through is limited to those that have been categorized as fruit. 
To determine which set of objects we are working with and which attribute we want to find we search through the CATEGORIES table and the ATTRIBUTES table and look for matching words in the sentence. 

These examples should serve as a starting point for adding onto NEO’s current set of abilities. 


