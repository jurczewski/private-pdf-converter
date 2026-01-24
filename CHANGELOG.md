# 🔑🔒 Changelog

## 1.2.1 - 2026.01.24 - Security fixes & standalone executables

### Security

- **security:** fix Magick.NET vulnerability (upgrade to 14.10.2)
- **security:** pin GitHub Actions to commit SHA for improved supply chain security

### Features

- **feat:** add standalone executable releases for Windows, Linux, and macOS (x64 and ARM64)
- **feat:** add manual workflow dispatch for building releases on-demand

### Fixes

- **fix:** disable PublishTrimmed to prevent Cocona reflection issues
- **fix:** improve cross-platform PowerShell compatibility in workflows
- **fix:** update PackageReleaseNotes to link to changelog on GitHub
- **fix:** update IsImage method to handle null and empty extensions

### Improvements

- **improve:** replace custom AddFileToPath with built-in Path.Combine
- **improve:** add unit tests for FileService and standardize test naming

### Documentation

- **docs:** add standalone executable installation instructions
- **docs:** improve grammar and consistency in documentation

## 1.2.0 - 2026.01.16 - .NET 10 LTS & auto output naming

### Features

- **feat:** auto-detect output filename from the source file (defaults to source name when no output is specified)

### Core Changes

- **chore:** upgrade .NET SDK version to 10.0.x across workflows and project files
- **fix:** make changelog on nuget.org to read from the CHANGELOG.md file directly (fix PackageReleaseNotes path)

### Maintenance

- **chore:** bump dependencies: itext and Magick.NET dependencies - fix vulnerability in Magick
- **chore:** improve comments and simplify the AddFileToPath method
- **chore:** update NuGet badge style to match others
- **fix:** "Buy Me a Coffee" badge link
- **chore(ci):** update NuGet vulnerability scan schedule - Runs at 09:00 UTC, only on Wednesday
- **chore(ci):** upgrade GitHub Actions: checkout@v2→v4, setup-dotnet@v1→v5 in deploy workflow
- **docs:** add a version check command example to README
- **improve:** add validation to prevent creating empty PDFs when no images are found in the directory
- **chore:** update package tags with more specific keywords

## 1.1.1 - 2025.10.08 - fix readme

- Readme fix: misspell
- Readme format

## 1.1.0 - 2025.10.08 - vulnerabilities fix release

- Resolve NuGet vulnerabilities
- Resolve deprecated NuGet packages
- Add a daily security-scan pipeline
- Fix README.md on NugetGallery
- Drop support for .NET 6
- Bump other dependencies
- Csproj general cleanup
- Use Central Package Management
- Better nuget/github/readme description

## 1.0.2 - 2025.10.06

- Move from FluentAssertions to AwesomeAssertions due to changes in the license model
- Update Magick.NET to adres vulnerabilities
- Use GlobalUsings for tests
- Bump other dependencies

## 1.0.1 - 2025.06.07

- Add a proper Continue Deployment pipeline for dotnet tool release/publish
- Drop old .NETs support (netcoreapp3.1;net5.0;net7.0;)
- Code quality: Move solution items to dir
- Add FUNDING.yml

## 1.0.0 - 2024.06.24

Initial full release of a stable product
