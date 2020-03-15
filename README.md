[![license](https://img.shields.io/github/license/rfadeev/pump-editor.svg)](https://github.com/rfadeev/pump-editor/blob/master/LICENSE.md)

# Pump Editor
Collection of Unity editor helpers to boost productivity.

## Installation
Project supports Unity Package Manager. To install project as Git package do following:
1. Close Unity project and open the `Packages/manifest.json` file.
2. Update `dependencies` to have `com.rfadeev.pumpeditor` package:
```json
{
  "dependencies": {
    "com.rfadeev.pumpeditor": "https://github.com/rfadeev/pump-editor.git"
  }
}
```
3. Open Unity project.

Alternatively, add this repository as submodule under `Assets` folder or download it and put to `Assets` folder of your Unity project. 

### How to use
Access Pump Editor editor windows via Unity toolbar: **Window -> Pump Editor**.

### Features

Editor windows:
* [Built-in Define Directives Editor Window](https://github.com/rfadeev/pump-editor/wiki/Built-in-define-directives-editor-window) - view Unity built-in defines values at editor time.
* [Force Reserialize Assets Editor Window](https://github.com/rfadeev/pump-editor/wiki/Force-Reserialize-Assets-Editor-Window) - force reserialize project assets explicitely.
* [Prefab Variants Editor Window](https://github.com/rfadeev/pump-editor/wiki/Prefab-Variants-Editor-Window) - preview prefab variants inheritance chains as tree view.
* [Project Settings Select Editor Window](https://github.com/rfadeev/pump-editor/wiki/Project-Settings-Select-Editor-Window) - select project settings to edit in one click.
* [Save Project As Template Editor Window](https://github.com/rfadeev/pump-editor/wiki/Project-templates-editor-windows) - save current project as template to be used in Unity Hub for new project creation.
* [Scene Open Editor Window](https://github.com/rfadeev/pump-editor/wiki/Scene-Open-Editor-Window) - open scene from project or build settings.

Shortcuts:
* [Lock Toggle Shortcuts](https://github.com/rfadeev/pump-editor/wiki/Lock-Toggle-Shortcuts) - switch editor window locks easily.
