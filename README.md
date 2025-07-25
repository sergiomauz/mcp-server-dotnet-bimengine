# A MCP server project - To be shown to Aurora (BIM Engine)

> This project implements a simple application to test SDK for .Net to create a MCP Server, to be used by Claude, Copilot or another LLM.
> The idea of this test is to have a screenshot of a running program (in this case Firefox browser) using Windows API, then apply an OCR to recognize a text (in this case, the domain in the address bar).

## Screenshot

![screenshot-1](https://i.imgur.com/chNP4Db.png)

## Features

- Console application.
- 1 MCP Server with 2 tools.
- 1 JSON file with configurations to add to Claude Desktop (**claude_desktop_config.json**).

## Built With

- Visual Studio 2022 - Community Edition
- C# / .Net 8
- ModelContextProtocol - [Repository here](https://github.com/modelcontextprotocol/csharp-sdk)
- Tesseract and Tessdata - [Repository here](https://github.com/tesseract-ocr/tessdata)

## Used Techniques

- Clean architecture
- Clean code
- Static and instanced classes
- Handling exceptions
- Dependencies injection

## How to Install

- Install Visual Studio 2022 Community Edition and .Net 8 in your computer.
- Install Claude Desktop.
- Install Firefox Browser.
- Create a new directory in C:\ -> **C:\revit-mcp-server**. It will be the workspace for this project. Make sure to be there in your terminal.
- Create a new directories inside the workspace, named **captures**, **app** and **tessdata**. Make sure to have permissions to write in all directories that you created.
- In **captures** directory all screenshots will be saved.
- Clone this repository directly to **app**: ``git clone https://github.com/sergiomauz/mcp-server-dotnet-bimengine.git app``
- Clone tessdata repository directly to **tessdata**: ``git clone https://github.com/tesseract-ocr/tessdata.git tessdata``
- Finally, click in Build Solution (F6) to make sure that you will have the an .exe file to be used by Claude; if there are no errors, you did it very well.

## How to Use

There are 2 ways to use this application: As a console application and as a MCP Server.

To be used as a console application:

- Open a Firefox window (install it if you don't have it) and navigate to any website. In this case [BIM-Engine](https://bimengine.ai/) ![screenshot-3](https://i.imgur.com/5miq1zU.png)

- In VS 2022, open **program.cs** file, comment region "REVIT-MCP-SERVER" and uncomment region "CONSOLE-APPLICATION". ![screenshot-2](https://i.imgur.com/9jzFmpy.png)

- Run application (F5), wait until application is ready: ![screenshot-4](https://i.imgur.com/zMVwJ6B.png)

- Put the Firefox window and the website in the foreground to continue, wait 2 seconds and application will show a message with the domain that it detected: ![screenshot-5](https://i.imgur.com/UCQPD0T.png)

- In the directory ``C:\revit-mcp-server\captures`` you can see 2 new files: A screenshot of Firefox and a cropped image focused in the address bar: ![screenshot-6](https://i.imgur.com/Q8x338O.png)

- If you don't put any Firefox window in foreground after 10 seconds, the application will send a message and terminate. ![screenshot-7](https://i.imgur.com/Sqb35k6.png)

To be used as a MCP server:

- Open a Firefox window (install it if you don't have it) and navigate to any website. In this case [BIM-Engine](https://bimengine.ai/) ![screenshot-8](https://i.imgur.com/5miq1zU.png)

- In VS 2022, open **program.cs** file, uncomment region "REVIT-MCP-SERVER" and comment region "CONSOLE-APPLICATION". ![screenshot-9](https://i.imgur.com/JK8XTwZ.png)

- Run application (F5) and wait until application is ready, if all is ok without errors and exceptions, you can stop it: ![screenshot-10](https://i.imgur.com/pfwhHyv.png)

- In your file explorer, go to ``%APPDATA%\Claude``, find the file **claude_desktop_config.json** to edit. Add the configuration to run a new MCP Server using the content of **claude_desktop_config.json** (the other one in **C:\revit-mcp-server\app** directory). ![screenshot-11](https://i.imgur.com/6BhLyyH.png)

- Finally, open Claude Desktop, if there are no problems, you can see a new tool in your toolbox. ![screenshot-12](https://i.imgur.com/8DPCXaT.png)

- Now you can ask for a task to your Claude. Send a message asking for the domain that is opened in your Firefox (it could take some seconds to detect and run, and remember to put the Firefox window and the website in the foreground). You can see the results in the chat and **captures** directory:
![screenshot-13](https://i.imgur.com/IBie0LO.png)
![screenshot-14](https://i.imgur.com/MJZCK0f.png)

## Author

👤 **Sergio Zambrano**

- Github: [@sergiomauz](https://github.com/sergiomauz)
- Twitter: [@sergiomauz](https://twitter.com/sergiomauz)
- Linkedin: [Sergio Zambrano](https://www.linkedin.com/in/sergiomauz/)

## 🤝 Contributing

Contributions, issues and feature requests are welcome!. Feel free to check the [issues page](../../issues/).

## Show your support

Give a ⭐️ if you like this project!

## 📝 License

This project is [MIT](./LICENSE) licensed.
