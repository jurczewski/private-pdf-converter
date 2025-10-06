# PrivatePdfConverter - LLM Documentation

## Project Overview

PrivatePdfConverter is a .NET CLI tool designed for secure, offline PDF operations. It's a cross-platform console application that prioritizes privacy by keeping all operations local without requiring cloud services.

### Key Features

- **Image to PDF Conversion**: Convert individual images or entire directories to PDF
- **PDF Manipulation**: Merge multiple PDFs, encrypt with password protection, add logos/watermarks
- **Privacy-First**: All operations performed offline
- **Cross-Platform**: Targets .NET 6.0 and .NET 8.0
- **CLI Interface**: Simple command-line interface using Cocona framework

## Architecture & Technology Stack

### Core Technologies

- **Framework**: .NET 6.0 & .NET 8.0
- **CLI Framework**: Cocona.Lite (v2.2.0)
- **PDF Processing**: iText7 (v8.0.4) with BouncyCastle adapter
- **Image Processing**: Magick.NET-Q8-AnyCPU (v13.9.1)
- **Logging**: Serilog (v4.0.0)
- **Testing**: xUnit with AwesomeAssertions, AutoFixture, and Moq

### Project Structure

```
├── src/
│   └── PrivatePdfConverter/           # Main application
│       ├── Commands/                  # Command implementations
│       │   ├── AddLogoToPdf.cs       # Logo/watermark functionality
│       │   ├── DirToPdf.cs           # Directory to PDF conversion
│       │   ├── EncryptPdf.cs         # PDF encryption
│       │   ├── ImgToPdf.cs           # Single image to PDF
│       │   ├── ListValidExt.cs       # List supported extensions
│       │   └── MergePdf.cs           # PDF merging
│       ├── Services/
│       │   └── FileService.cs        # File operations utilities
│       ├── Logger.cs                 # Logging configuration
│       ├── Program.cs                # Entry point and command registration
│       └── PrivatePdfConverter.csproj # Project configuration
├── tests/
│   └── PrivatePdfConverter.Tests/    # Test project
│       ├── IntegrationTests/         # Integration tests
│       └── UnitTests/                # Unit tests
├── assets/                           # Logo and branding assets
├── scripts/                          # Build and deployment scripts
└── .github/workflows/                # CI/CD configuration
```

## Command Line Interface

The tool is installed as a global .NET tool with the command name `ppc`. All commands follow this pattern:

```
ppc <command> --<option> <value>
```

### Available Commands

#### 1. `dir` - Convert Directory to PDF

Converts all supported images in a directory to a single PDF.

```bash
ppc dir --path "C:\images" --output "combined.pdf"
```

**Parameters:**

- `path` (required): Directory containing images
- `output` (optional): Output PDF filename (default: "output.pdf")

#### 2. `img` - Convert Single Image to PDF

Converts a single image file to PDF.

```bash
ppc img --path "C:\image.png" --output "converted.pdf"
```

#### 3. `merge` - Merge PDF Files

Merges all PDF files in a directory into a single PDF.

```bash
ppc merge --path "C:\pdfs" --output "merged.pdf"
```

#### 4. `encrypt` - Encrypt PDF

Encrypts a PDF file with password protection.

```bash
ppc encrypt --path "C:\document.pdf" --password "secret123" --output "encrypted.pdf"
```

#### 5. `logo` - Add Logo/Watermark

Adds an image as a watermark to every page of a PDF.

```bash
ppc logo --path "C:\document.pdf" --logo-path "C:\logo.png" --position "bottom-right" --scale 25 --opacity 50
```

**Parameters:**

- `position`: "top-left", "top-right", "bottom-left", "bottom-right"
- `scale`: Percentage (e.g., 25 for 25%)
- `opacity`: Percentage (e.g., 50 for 50% transparency)

#### 6. `ext` - List Supported Extensions

Lists all supported image file extensions.

### Supported Image Formats

- jpg, jpeg
- png
- bmp
- gif
- tif, tiff
- webp

## Core Components

### 1. Program.cs - Application Entry Point

- Initializes Serilog logging
- Registers all commands with Cocona framework
- Manages application lifecycle

### 2. Commands Directory

Each command is implemented as a static class with a main method that Cocona can invoke:

#### AddLogoToPdf.cs

- Calculates logo positioning based on PDF dimensions
- Supports scaling and opacity adjustments
- Adds logos to all pages in the PDF

#### DirToPdf.cs & ImgToPdf.cs

- Uses Magick.NET for image processing
- Filters files by supported extensions
- Creates MagickImageCollection and exports to PDF

#### EncryptPdf.cs

- Uses iText7 for PDF encryption
- Implements AES-128 encryption with user/owner passwords
- Supports standard PDF encryption permissions

#### MergePdf.cs

- Reads multiple PDF files from directory
- Uses iText7 to copy pages between documents
- Creates single merged output PDF

### 3. Services/FileService.cs

Utility methods for file operations:

- Extension validation (`IsImage()`)
- File path loading from directories
- Output filename preparation (ensures .pdf extension)
- Path manipulation utilities

### 4. Logger.cs

- Configures Serilog with console output
- Uses AnsiConsoleTheme for colored output
- Logs application version and commit information

## Build & Deployment

### Project Configuration (PrivatePdfConverter.csproj)

- Multi-targeting: .NET 6.0 and .NET 8.0
- Packaged as global .NET tool (`PackAsTool=true`)
- Command name: `ppc` (`ToolCommandName=ppc`)
- Includes README, CHANGELOG, and assets in package

### NuGet Package

- Package ID: PrivatePdfConverter
- Published to NuGet.org
- Includes symbols package for debugging
- Semantic versioning

### CI/CD Pipeline (.github/workflows/)

- **build.yaml**: Runs on push/PR to master, executes tests
- **deploy.yaml**: Manual deployment to NuGet

## Testing Strategy

### Test Structure

- **Unit Tests**: Individual component testing (e.g., position calculations)
- **Integration Tests**: End-to-end command testing with real files

### Test Dependencies

- **xUnit**: Test framework
- **AwesomeAssertions**: Assertion library for readable tests
- **AutoFixture**: Test data generation
- **Moq**: Mocking framework
- **ImageMagick**: For creating test images

### Test Examples

```csharp
// Unit test for logo positioning
[Theory]
[InlineData("top-left")]
[InlineData("bottom-right")]
public void CalculatePosition_ShouldBehaveCorrectly(string position)

// Integration test for directory conversion
[Fact]
public void ConvertDirectoryToOnePdf_ShouldBehaveCorrectly()
```

## Development Guidelines

### Code Style

- Uses comprehensive .editorconfig with C# formatting rules
- SonarQube rules for code quality
- StyleCop rules (with some disabled for flexibility)

### Dependencies

- **Cocona.Lite**: Lightweight CLI framework for .NET
- **iText7**: Industry-standard PDF manipulation library
- **Magick.NET**: .NET wrapper for ImageMagick
- **Serilog**: Structured logging library

### Error Handling

- Validates file existence before operations
- Checks file extensions against supported formats
- Provides informative error messages via logging

## Usage Patterns for LLMs

When working with this codebase, consider these patterns:

### Adding New Commands

1. Create new class in `Commands/` directory
2. Implement static method with Cocona-compatible signature
3. Register command in `Program.cs`
4. Add tests in appropriate test directory

### File Processing Pattern

```csharp
// Common pattern for file operations
var files = path.LoadFilePathsFromDirectory(searchPattern);
var validFiles = files.Where(x => Path.GetExtension(x).IsImage());
// Process files...
var outputFileName = output.PrepareOutputFileName();
Log.Logger.Information("Operation completed: {OutputFileName}", outputFileName);
```

### PDF Operations Pattern

```csharp
// iText7 pattern for PDF manipulation
using var pdfReader = new PdfReader(inputPath);
using var pdfWriter = new PdfWriter(outputPath);
using var pdfDocument = new PdfDocument(pdfReader, pdfWriter);
// Perform operations...
```

### Image Processing Pattern

```csharp
// Magick.NET pattern for image operations
using var images = new MagickImageCollection();
images.Add(new MagickImage(imagePath));
images.Write(pdfOutputPath);
```

This documentation provides comprehensive context for LLMs to understand the codebase structure, functionality, and development patterns used in the PrivatePdfConverter project.
