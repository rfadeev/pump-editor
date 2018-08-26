[![license](https://img.shields.io/github/license/rfadeev/pump-editor.svg)](https://github.com/rfadeev/pump-editor/blob/master/LICENSE.md)

# Pump Editor
Collection of Unity editor helpers to boost productivity.

## Project Settings Select Editor Window
![project-settings-select-editor-window](https://user-images.githubusercontent.com/5451929/44627227-7f24be80-a95c-11e8-8fb7-ec8679de014a.gif)
![editor-window-show-inspector](https://user-images.githubusercontent.com/5451929/44627273-9021ff80-a95d-11e8-88b0-f4e46ea79898.jpg)

### Motivation
Normally to edit Unity project settings user have to navigate there via **Edit->Project Settings->Item To Edit**. Having project settings item to edit shown in inspector, selecting any other object will switch inspector, so user have to navigate again to edit project settings item. To ease the pain of navigation and accidental misselections, project settings select editor window was implemented. It allows to select Unity project settings item in single click instead of navigating via menu. Editor window supports showing target project settings item inspector inside window to edit it directly. Side by side editor window and default inspector is an alternative use case.

### How to use
Add this repository as submodule under Assets folder or download it and put to Assets folder of your Unity project. Access project settings select window via Unity toolbar: **Window->Pump Editor->Project Settings Select**.
