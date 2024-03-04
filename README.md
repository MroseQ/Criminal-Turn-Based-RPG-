<h1 align="center">CRIMINAL (Turn-Based Combat) <i>[DISCONTINUED]</i></h1>
<h4 align="center">using C# Unity <br>by Marek Krasiński<br>ONLY IN POLISH LANGUAGE</h4>
<h3 align="center"><i>Turn-based combat game with only one fight. Four different allies and five different enemies. </i><br> --- inspired by Sonny</h3>

<h6 align="center">DISCLAIMER: Some of the assets used in this project are not of my authorship, and I do not possess full copyright. These assets have been used solely for narrative and demonstrative purposes. All copyrights belong to their rightful owners. If you are the copyright owner of any of the used assets and wish for them to be removed or appropriately identified, please get in touch with me. </h6> 

<h2>
 Table of Contents:
</h2>


1. [Scenes](#scenes)
   1. [Team Setup](#setup)
   2. [Fighting Scene](#fight)
   3. [Endscreen](#endScreen)
2. [Effects](#effects)


<br>
<h1>Scenes<a name="scenes"></h1>

<h2>Team Setup</h2><a name="setup"></a>

<div align="center" width="50%">

   ![Setup](https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/b280f46b-60c7-4b23-9f28-cbc8f0a07ae3)
  
  There are four different allies to choose from. You must choose two and only two. Every ally has their own "class" and their special abilities.
  
</div>
<br>
<h2>Fighting Scene</h2><a name="fight"></a>

<div align="center" width="50%">
 
   ![Intro](https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/5c73b277-4ea8-43cb-917e-f7a1f74229c1)
   
   The game has two sides fighting each other: Player characters (Left side) and Enemy characters (Right side).
   To win the game, you need to eliminate every enemy (reduce their Health Bar to 0) without losing all Health Points on every one of your characters.
   Every character has its own Health Bar and Mana Bar visible on the upper part of the scene.
   Every character must select a skill they want to use on their turn. 
   Some skills have their Mana Cost (which will reduce the character's Mana Points) and/or have their Skill Cooldown (the number of turns that the character must wait before using the skill again).
   Some skills may require additional information about the target on which the skill should be used. To provide this information, the player should click two times on a target.
   
   ![SkillDesc](https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/a349fbdf-599c-4e93-b446-807ceabd080c)
   
   When certain conditions are met, two new enemies will appear.
   
   ![MoreEnemies](https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/e72a302a-78ff-46be-97b1-205b5c21c476)

</div><br>

<h2>End Screen</h2><a name="endScreen"></a>

<div align="center" width="50%">

   Example of losing all of your characters:
   
   ![Die and endscreen](https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/3f10b7fd-806a-4233-bc64-103ea40000e8)
 
</div><br>


<h1>Effects</h1> <a name="Effects"></a>

<div align="center">

Some skills can inflict effects on characters. Effects could be defensive, supportive, negative, or informative. Inflicted effects of a character are placed on Mana Bar. 
Hovering over the effect will display the name, description, and sometimes duration.

<img src="https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/6253906f-c89b-495f-86b2-30e4247d8874/EffectDesc" width="30%">

</div>

<div display="inline" align="center">
<p align="center">Example of Effects altering the game:</p>

<img margin-right="5%" src="https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/3f68a902-b4d6-4fb4-828f-103fd943456f/EffectShowcase2">
<img padding="5%" src="https://github.com/MroseQ/Criminal-Turn-Based-RPG-/assets/46853552/39d521e8-5cb3-4518-bb52-0365636db3e4/EffectShowcase1">

</div>

<h3 align="center">The game's code was created solely by myself.</h3>

<h2 align="center">Game and project view available on request.</h2>
