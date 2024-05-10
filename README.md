# LOD Group Prefab Creator

## Overview

The **LOD Group Prefab Creator** is a Unity Editor tool that automates the process of creating Level of Detail (LOD) prefabs directly from the project view. This tool lets you select multiple LOD meshes, customize LOD settings, and generate an LOD group prefab with ease. The generated prefab is then saved to your desired folder location.

## Why Use It?

1. **Efficiency**: Automatically creates an LOD group prefab with all LOD meshes and thresholds set up.
2. **Customizability**: Offers control over LOD thresholds, collider settings, and material assignment.
3. **Consistency**: Apply uniform settings across multiple prefabs to ensure consistent behavior and visuals.
4. **Simplicity**: Streamlines the creation of optimized prefabs with intuitive configuration directly in the Unity Editor.
5. **Persistence**: Remembers your settings even after closing the tool, so you can pick up where you left off.

## How It Works

- **LOD Thresholds**: Automatically initializes default thresholds based on the number of selected meshes.
- **Colliders**: Optionally adds a MeshCollider to LOD0 or the root object for efficient collision detection.
- **Materials**: Assign a single material or list of materials uniformly to all LOD meshes.
- **Persistence**: Uses `EditorPrefs` to persist your last-used settings, saving you time on configuration.

## How to Use

1. **Open the Tool**: From the Unity menu bar, go to `Tools -> LOD Group Prefab Creator`.
2. **Select LOD Meshes**: In the Project view, select the desired meshes for your LOD group.
3. **Configure Settings**:
   - **Prefab Name**: Enter the desired prefab name.
   - **LOD Thresholds**: Adjust the sliders to set the transition thresholds.
   - **Collider Options**:
     - **Add Collider to LOD0**: Option to add a MeshCollider to LOD0.
     - **Place Collider on Root**: Places the collider on the root object instead of LOD0.
   - **Materials**: Specify the number of materials, then drag-and-drop them into the slots.
   - **Save Folder**: Click `Select Save Folder` to choose where to store the prefab.

4. **Create Prefab**: Click `Create LOD Prefab` to generate and save the prefab.

## Example Usage

1. **Creating a New LOD Prefab**: Quickly select several meshes, configure settings, and click `Create LOD Prefab` to save an optimized prefab in the specified folder.
2. **Reuse Settings**: After creating the first prefab, reopen the tool to see your last-used settings, reducing the time spent reconfiguring.

## Requirements

- Unity 2019.4 or newer.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
