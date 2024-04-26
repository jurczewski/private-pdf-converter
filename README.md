# 🔎 PrivatePdfConverter

## Description

Private PDF Converter is a secure offline tool designed to effortlessly convert PNG files stored locally into a comprehensive PDF document. With a user-friendly terminal interface, this tool prioritizes privacy by ensuring all conversion processes occur offline, eliminating any reliance on cloud services. Stay in control of your sensitive data as you seamlessly transform images into PDFs without compromising confidentiality. Future updates will introduce additional features, such as PDF splitting, to further enhance functionality (see roadmap below).

# Arguments

| Argument  | Description                                                     | Options                          | Optional options           |
| --------- | --------------------------------------------------------------- | -------------------------------- | -------------------------- |
| `dir`     | Converts all images inside a directory to a single PDF file.    | `path` - path to directory       | `output` - PDF output name |
|           | See below supported file extensions.                            |                                  |                            |
| `img`     | Converts a single image to a single PDF file.                   | `path` - path to image           | `output` - PDF output name |
|           | See below supported file extensions.                            |                                  |                            |
| `merge`   | Merges all PDF files inside a directory into a single PDF file. | `path` - path to directory       | `output` - PDF output name |
|           |                                                                 |                                  |                            |
| `encrypt` | Encrypts PDF with a password.                                   | `path` - path to a PDF file      | `output` - PDF output name |
|           |                                                                 | `password` - encryption password |                            |
| `ext`     | Lists all valid image extensions.                               |                                  |                            |

For all commends, default export .pdf file name is `output.pdf`.

### Supported file extensions

Type `ext` argument to list all of valid image extensions.

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
- [x] convert PDF to multiple images (`img`)
- [x] multiple image extensions support
- [x] merge pdf (`merge`)
- [ ] split pdf (`split`)
- [ ] encrypt pdf with password
- [ ] add watermark/logo to your pdf
- [ ] better TUI (menu)
- [ ] set default export dir
- [x] list all valid image extensions (`ext`)

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
