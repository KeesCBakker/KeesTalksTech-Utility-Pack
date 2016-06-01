# KeesTalksTech Utility Pack

After writing some code at [my blog KeesTalksTech.com][3] and having some code snippets laying around I decided to bundle them into a utility DLL. Makes things way easier to reference.

## Version 0.2
- **MetafileUtility**<br/>Helps with saving EMF and WMF files to PNG or JPG (or any desired format supported by .Net).
- **WebCacheCow**<br/>Provides a way to cache asynchronous requests. Very usable in data services that are asynchronous and need caching.
- **StreamUtility**<br/>Compares two streams.

## Version 0.1

- **AppSettingsProvider**<br/>Provides app settings from the .config to the application. Can be used to automatically fill settings objects using the convention: {namespace}.{class-name}.{field-name} <br/>Check: [Auto fill settings objects with .config values][4]
- **BetterWebClient**<br/> A (slightly) better version of .Net's default WebClient. The extra features include: ability to disable automatic redirect handling, sessions through a cookie container, indicate to the webserver that GZip compression can be used, exposure of the HTTP status code of the last request, exposure of any response header of the last request, ability to modify the request before it is send. <br/> Check: [A (slightly) better WebClient class][5].
- **BlockHasher**<br/>Helps with hashing streams, strings and other stuff.
- **Evaluator**<br/>Helps with the generation of scripts into classes. It will compile and execute them. Uses an `ICompiler` object. <br/> Check: [An evaluator for simple script evaluation][2].
- **ICompiler**<br/>Interface for compilation. Implemented C# compilation through MS Code DOM and the Roslyn project.<br/> Check: [How to add dynamic compilation to your c# projects?][1].

[1]:https://keestalkstech.com/2016/05/how-to-add-dynamic-compilation-to-your-projects/
[2]:https://keestalkstech.com/2016/05/an-evaluator-for-simple-script-evaluation/
[3]:https://keestalkstech.com
[4]:https://keestalkstech.com/2016/03/auto-fill-settings-objects-with-config-values/
[5]:https://keestalkstech.com/2014/03/a-slightly-better-webclient-class/