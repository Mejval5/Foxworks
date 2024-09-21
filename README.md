# Foxworks - Unity Common Utilities

**Foxworks** is a Unity submodule designed to provide a collection of common utilities, components, and systems to streamline game development. It includes a wide variety of tools for handling rendering, event delegation, analytics, UI/UX enhancements, and much more.

## Features

- **Analytics**: Event tracking and properties management to monitor player actions.
- **App Utilities**: Core systems like entry points and performance handlers for managing your Unity app’s lifecycle.
- **Attributes**: Custom Unity attributes for better editor tools (e.g., `ButtonDrawer` for drawing buttons in the inspector).
- **Components**: Reusable components like `FollowCanvasVisibility`, `ParticlesFollowCanvasVisibility`, and `FollowTransformWithOffset` for managing UI and transforms.
- **Editor Tools**: Custom editor scripts like `PrefabRefactoringEditor`, `ForceSaveAssetsTool`, and `SearchEverywhere` to improve productivity in the Unity Editor.
- **Event Delegation**: Easily manage in-game events with utilities like `TriggerDelegator2D`.
- **File Management**: Open and manage folders with ease using `FolderOpener` and other file-related utilities.
- **Rendering**: Includes utilities like `MaterialPropertyOverride` for fine-tuning rendering properties and materials.
- **UI/UX Enhancements**: Support for safe area adjustments (`SafeArea.cs`), vibration handling (`VibrationsHandler`), and more.
- **Utilities**: A vast collection of utility functions for common tasks such as math, string manipulation, lists, enums, and dictionaries.

## Project Structure

The **Foxworks** submodule is organized into several folders, each focusing on different aspects of Unity development:

- **Analytics**: Track events and collect relevant data.
- **App**: Core application utilities, like the main entry point and performance handling.
- **Attributes**: Custom attributes for editor tooling.
- **Components**: Reusable scripts for Unity components such as canvas and particle systems.
- **Editor**: Tools to enhance the Unity Editor, like asset refactoring and search utilities.
- **EventDelegation**: Event systems that handle triggers and delegate actions.
- **Files**: Tools for file handling and operations.
- **General**: General-purpose utilities, like `NameGenerator`.
- **Interfaces**: Interfaces for implementing systems like damage and entity interaction.
- **Juicing**: Effects such as `RandomBounce` and `LightFluctuate` for adding polish to game elements.
- **Noise**: A `SimplexNoiseGenerator` for procedural content generation.
- **Persistence**: Data saving and loading utilities.
- **Rendering**: Tools for managing material properties and overriding rendering behavior.
- **Sound**: A `SoundManager` for handling in-game audio.
- **UI**: Utilities for managing UI elements, such as safe area adjustments.
- **Utils**: A comprehensive library of general-purpose utilities (math, strings, lists, enums, etc.).
- **UX**: User experience improvements, including `VibrationsHandler`.
- **Version**: Tools for handling versioning, such as `SemVersion`.
- **Voxels**: Utilities for voxel-based rendering, including `MarchingCubeUtils` and `VoxelRaycaster`.

## Installation

To include **Foxworks** in your Unity project as a submodule:

```bash
git submodule add https://github.com/yourusername/Foxworks.git Assets/Foxworks
```

## How to Use

- **Analytics**: Track player actions and events using the `Analytics` system.
- **Editor Tools**: Use the custom tools in the Unity Editor to refactor assets, force save, and search for elements in your project.
- **UI Components**: Ensure your game UI adjusts for different screen sizes using the `SafeArea` component.
- **Rendering**: Override material properties at runtime with `MaterialPropertyOverride`.

## Contribution

Contributions are welcome! Feel free to open issues or submit pull requests for enhancements or bug fixes.

## License

This submodule is licensed under the MIT License.
