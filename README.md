
Project settings:
It is designed for Android so please setup unity build settings to Android and make sure in player setting scripting background to il2cpp & Target Architecture  arm64  is selected

In-Project Components:
ARCamera: Vuforia Camera Arbackground and model tetection
Canvas:
Load button: File selector and shows progress of model downloaded.
Close button: To close the app
Reset button: To reset tracked object scale/rotation
Load Model:
Empty gameobject for script reference
Touch script: Handle touch on the screen to scale/Rotate
Load Model: All button functions and load 3d files.
Model Target: Vuforia mode detector and parent object to attach 3d file which needs to be augmented.

