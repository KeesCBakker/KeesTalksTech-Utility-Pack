# KeesTalksTech Utility Pack

After writing some code at [my blog KeesTalksTech.com][3] and having some code snippets laying around I decided to bundle them into a utility DLL. Makes things way easier to reference.

## Version 0.2
- **[BoundingBox][15]**<br/>A conceptual box that can be used to calculate new boxed based on the same resolution.
- **[CacheCow][6]**<br/>Provides a way to cache asynchronous requests. Very usable in data services that are asynchronous and need caching.
- **[MetafileUtility][13]**<br/>Helps with saving EMF and WMF files to PNG or JPG (or any desired format supported by .Net).<br/>Check: [Rasterizing EMF files with C# / .Net][16]
- **[MVC Simple Forms Authentication][17]**<br/>Simple - but obsolete - forms authentication where user, password and roles are stored in the web.config.<br/>Check:[Simple Database-less Authentication for MVC.Net][18]
- **[StreamUtility][14]**<br/>Compares two streams.

## Version 0.1

- **[AppSettingsProvider][12]**<br/>Provides app settings from the .config to the application. Can be used to automatically fill settings objects using the convention: {namespace}.{class-name}.{field-name} <br/>Check: [Auto fill settings objects with .config values][4]
- **[BetterWebClient][11]**<br/> A (slightly) better version of .Net's default WebClient. The extra features include: ability to disable automatic redirect handling, sessions through a cookie container, indicate to the webserver that GZip compression can be used, exposure of the HTTP status code of the last request, exposure of any response header of the last request, ability to modify the request before it is send. <br/> Check: [A (slightly) better WebClient class][5].
- **[BlockHasher][7]**<br/>Helps with hashing streams, strings and other stuff.<br/>Check: [A BlockHasher helper class][8].
- **[Evaluator][9]**<br/>Helps with the generation of scripts into classes. It will compile and execute them. Uses an `ICompiler` object. <br/> Check: [An evaluator for simple script evaluation][2].
- **[ICompiler][10]**<br/>Interface for compilation. Implemented C# compilation through MS Code DOM and the Roslyn project.<br/> Check: [How to add dynamic compilation to your c# projects?][1].

[1]:https://keestalkstech.com/2016/05/how-to-add-dynamic-compilation-to-your-projects/
[2]:https://keestalkstech.com/2016/05/an-evaluator-for-simple-script-evaluation/
[3]:https://keestalkstech.com
[4]:https://keestalkstech.com/2016/03/auto-fill-settings-objects-with-config-values/
[5]:https://keestalkstech.com/2014/03/a-slightly-better-webclient-class/
[6]:KeesTalksTech-Utility-Pack/blob/master/KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Caching/CacheCow.cs
[7]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Hashing/BlockHasher.cs
[8]:https://keestalkstech.com/2016/05/a-block-hasher-helper-class/
[9]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Evaluation/Evaluator.cs
[10]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Compilation
[11]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Net/BetterWebClient.cs
[12]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Settings/AppSettingsProvider.cs
[13]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Graphics/MetafileUtility.cs
[14]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/IO/StreamUtility.cs
[15]:KeesTalksTech-Utility-Pack/KeesTalksTech.Utilities/Graphics/BoundingBox.cs
[16]:https://keestalkstech.com/2016/06/rasterizing-emf-files-with-net-c/
[17]:KeesTalksTech-Utility-Pack/tree/master/KeesTalksTech-Utility-Pack/KeesTalksTech.UtilityPack.Web/Mvc/Logon
[18]:https://keestalkstech.com/2016/06/simple-database-less-authentication-for-mvc-net/