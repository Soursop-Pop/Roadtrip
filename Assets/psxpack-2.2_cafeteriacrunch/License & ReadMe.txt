=====================
Detail ##############
=====================
Product: PSXPack by orange rice
Classification: Game asset
Description: 450+ Meshes & 70+ Textures for your projects!
Version: Bakery Update (5.00$+) - January 2025

=====================
License #############
=====================
When using the contents you DO NOT have to credit me/PSXPack, but I would highly appreciate it.
Example: Uses contents of PSXPack created by orange rice @ https://orange-rice.itch.io

You are allowed to use the contents for
 - commercial and non-commercial projects
 
You are not allowed to 
- redistribute or sell the contents in any form, regardless of it being a simple reupload or a modified version

Note: Multiple textures used with these 3D models have been created with photographs from Textures.com.
These photographs may not be redistributed by default; please visit www.textures.com for more information.
I am contineously working to decrease the contents originating from textures.com with future updates.

=====================
Installation ########
=====================
Within the downloaded files you get three folders, "/meshes", "/textures" and "/shaders".
In case of Godot and Unity just insert those folders into your project and you are good to go.
With Unreal I have no personal experience, but it should not be much more difficult than that either.

Apart from the original .blend-files for Blender 4.3 and higher, I also provide .OBJ-files for other engines. The later have the textures baked in them.

The models use the Principled BDSF shader, but no PBR-textures to mimic the limitations of PS1-technology.
Most models have a basic specular shine set to very low values that can be adjusted according to your own preferences.

The shaders in "/shaders" are written for Godot 4.3 and later. I do not guarantee their functionality in other engines.
For models with alpha values are materials provided (Godot only). They should be working by simply inserting them alongside the according textures into your Godot-project.

=====================
Questions & Answers #
=====================
-- Why are you using .PNG's with baked in alpha? --
Because Godot can not read .TGA's with alpha channels. At least it does not work during tests and despite claims on the Godot-Documentation that it does.
Once I figure it out or it is fixed I will start converting textures into more appropiate Image formats, if applicable.