# 🔎 PrivatePdfConverter

## Description

Private PDF Converter is a secure offline tool designed to effortlessly convert PNG files stored locally into a comprehensive PDF document. With a user-friendly terminal interface, this tool prioritizes privacy by ensuring all conversion processes occur offline, eliminating any reliance on cloud services. Stay in control of your sensitive data as you seamlessly transform images into PDFs without compromising confidentiality. Future updates will introduce additional features, such as PDF splitting, to further enhance functionality (see roadmap below).

# Arguments

| Argument | Description                                               | Options                    |
| -------- | --------------------------------------------------------- | -------------------------- |
| `dir`    | Converts all images inside directory to single pdf file.  | `path` - path to directory |
|          | See below supported file extensions.                      | `output` - pdf output name |
| `merge`  | Merge all .pdf files inside directory to single pdf file. | `path` - path to directory |
|          |                                                           | `output` - pdf output name |

### Supported file extensions

- .jpg
- .jpeg
- .bmp
- .gif
- .png
- .tif
- .tiff
- .webp

## Features roadmap

- [x] convert dir to pdf (`dir`)
- [ ] create a PDF file from a single image (`pdf`)
- [ ] convert PDF to multiple images (`img`)
- [x] multiple image extensions support
- [x] merge pdf (`merge`)
- [ ] split pdf (`split`)
- [ ] encrypt pdf with password
- [ ] add watermark/logo to your pdf
- [ ] better TUI (menu)
- [ ] set default export dir

### Other

- [x] readme
- [x] features roadmap
- [x] editorconfig
- [x] logging
- [x] license
- [x] pipeline
- [x] better readme
- [ ] exe release
- [ ] unit tests
- [ ] cool labels (version, build, license, buymecoffe)
- [ ] auto release from master [LINK](https://github.com/xoofx/dotnet-releaser)
- [ ] winget/chocolatey

# Development

TBA

## Dependencies

Based on micro-framework [Cocona](https://github.com/mayuki/Cocona).
