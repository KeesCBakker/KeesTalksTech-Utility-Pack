# KeesTalksTech Utilities

After writing some code at [my blog KeesTalksTech.com][3] and having some code snippets laying around I decided to bundle them into a utility DLL. Makes things way easier to reference.


## Version 0.1

- **BlockHasher**<br/>Helps with hashing streams, strings and other stuff.
- **ICompiler**<br/>Interface for compilation. Implemented C# compilation through MS Code DOM and the Roslyn project.<br/> Check: [How to add dynamic compilation to your c# projects?][1].
- **Evaluator**<br/>Helps with the generation of scripts into classes. It will compile and execute them. Uses an `ICompiler` object. <br/> Check: [An evaluator for simple script evaluation][2].
- **AppSettingsProvider**<br/>Provides app settings from the .config to the application. Can be used to automatically fill settings objects using the convention: {namespace}.{class-name}.{field-name} <br/>Check: [Auto fill settings objects with .config values][4]

[1]:https://keestalkstech.com/2016/05/how-to-add-dynamic-compilation-to-your-projects/
[2]:https://keestalkstech.com/2016/05/an-evaluator-for-simple-script-evaluation/
[3]:https://keestalkstech.com
[4]:https://keestalkstech.com/2016/03/auto-fill-settings-objects-with-config-values/