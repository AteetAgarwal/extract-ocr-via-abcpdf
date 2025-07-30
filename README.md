# ExtractOCR

## Overview
ExtractOCR is a .NET solution designed to extract images from PDF documents using the ABCpdf library. This project demonstrates how to leverage ABCpdf's capabilities to process PDF files and retrieve embedded images efficiently.

## Features
- Extract images from PDF documents.
- Supports various image formats embedded in PDFs.
- Utilizes the powerful ABCpdf library for PDF processing.

## Prerequisites
- Windows operating system.
- .NET 8.0 SDK installed.
- ABCpdf library dependencies included in the project.

## Getting Started

### Build and Run
1. Clone the repository to your local machine.
2. Open the solution file `ExtractOCR.sln` in Visual Studio.
3. Restore NuGet packages if required.
4. Build the solution to ensure all dependencies are resolved.
5. Run the application to start extracting images from PDF files.

### Usage
1. Place the PDF files you want to process in the appropriate directory.
2. Run the application and follow the prompts to extract images.
3. Extracted images will be saved in the designated output folder.

## Project Structure
- `Program.cs`: Entry point of the application.
- `bin/`: Contains compiled binaries and dependencies.
- `obj/`: Contains intermediate build files.
- `images/`: Directory for storing extracted images.

## Dependencies
- ABCpdf.NET library for PDF processing.
- .NET 8.0 runtime.

## License
This project is licensed under the MIT License.