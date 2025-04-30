# Java IDE (C#, Windows Forms, ScintillaNET)

A lightweight, beginner-friendly Java IDE built using C#, Windows Forms, and ScintillaNET. The IDE supports dark mode, syntax highlighting, bracket auto-closing, and one-click compile/run functionality for `.java` files.

## ✨ Features

- 🎨 **VS Code-inspired Dark Theme**  
- 🖍 **Syntax Highlighting** for Java using ScintillaNET  
- 🛠 **Compile & Run Support** using `javac` and `java`  
- 🧠 **Real-time Error Feedback** in output panel  
- 🧩 **Auto-closing Brackets & Quotes**  
- 💾 **File Save & Load Support**

## 🖼 Preview

![screenshot](screenshots/java-ide-demo.png)

## 🚀 Getting Started

### Prerequisites

- Windows OS
- [.NET Framework 4.7+](https://dotnet.microsoft.com/en-us/download/dotnet-framework)
- [Java JDK](https://www.oracle.com/java/technologies/javase-downloads.html)
- Visual Studio (for building the project)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/java-ide-winforms.git

2. Navigate to the project directory:
   ```bash
   cd java-ide-winforms
   
3. Open the solution file (JavaIDE.sln) in Visual Studio.

4. Build the project using Build > Build Solution or press Ctrl + Shift + B.

5. Run the project (F5) or use Debug > Start Debugging.

6. Ensure Java JDK is installed and both javac and java are accessible via Command Prompt. You can test this by running:

    ```bash
   javac -version
   java -version
