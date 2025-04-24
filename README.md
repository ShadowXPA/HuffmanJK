# HuffmanJK

Star Wars Jedi Knight: Jedi Academy's Huffman Coding/Decoding port to C#.

## Usage

To use this program, you will need the `dotnet` cli tool.
You will also need `.NET 8`.

Run the following command for all options:
```
dotnet run --project HuffmanJK -- help
```

## Encoding

Run the following command for all options:
```
dotnet run --project HuffmanJK -- encode help
```

File input:
```
dotnet run --project HuffmanJK -- encode -i "decoded.txt" -O 12
```

Base64 text input:
```
dotnet run --project HuffmanJK -- encode -t "ICAgIGNvbm5lY3QgIlxjaGFsbGVuZ2VcMTc3MDU3MDcxMVxxcG9ydFwyODg4N1xwcm90b2NvbFwyNlx0ZWFtb3ZlcmxheVwwXHNuYXBzXDQwXHNleFxtYWxlXHNhYmVyMlxub25lXHNhYmVyMVxkdWFsXzFccmF0ZVwyNTAwMFxwYXNzd29yZFwxMjNcbmFtZVwoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKCgoKFxtb2RlbFxpbXBlcmlhbC9jb21tYW5kZXJcamFfZ3VpZFw5ODc0Mzg2RTFCMjBCQ0Q0OEM4MTRBNTU2OEI2RkU5QVxoYW5kaWNhcFwxMDBcZm9yY2Vwb3dlcnNcNy0yLTAzMDAwMDAwMDAwMDAwMDMzMFxjcF9zYlJHQjJcMFxjcF9zYlJHQjFcMFxjcF9wbHVnaW5EaXNhYmxlXDBcY3BfY29zbWV0aWNzXDBcY3BfY2xhblB3ZFxub25lXGNvbG9yMlw0XGNvbG9yMVw0XGNqcF9jbGllbnRcMS40SkFQUk9cY2hhcl9jb2xvcl9yZWRcMjU1XGNoYXJfY29sb3JfZ3JlZW5cMjU1XGNoYXJfY29sb3JfYmx1ZVwyNTVcY2dfcHJlZGljdEl0ZW1zXDFcY2dfZGlzcGxheU5ldFNldHRpbmdzXDEyNSAwIDEyNVxjZ19kaXNwbGF5Q2FtZXJhUG9zaXRpb25cMCAxMjAgMTYi" -O 12
```

Standard input: (use `cmd.exe /c` if running on PowerShell)
```
dotnet run --project HuffmanJK -- encode --stdin -O 12 < decoded.txt
```

## Decoding

Run the following command for all options:
```
dotnet run --project HuffmanJK -- decode help
```

File input:
```
dotnet run --project HuffmanJK -- decode -i "encoded.bin" -O 12
```

Base64 text input:
```
dotnet run --project HuffmanJK -- decode -t "/////2Nvbm5lY3QgAhlEdDCOBQzHJsMU7I75ZzAadikYACv/tPtxLBwU+8CJcaHBhHCQf4rW8yfcTAm23HcNbDfhpls7x9PHAecOvbYaFig3hsdskdxwjFEPPuHhBpV6P6/HhMa1LEO/jSthsfoeT92jEHfm7H50YV6cYgZCQdnNTXqoEXUPyraTbf8PmZ8SsxKW95/rYswL9NenreuhwSusEg+DrRfP4dSlAjquDBHVQoRo7NhBWIXIgTBTj1YRvL97to5xEevsjlSNYXgXHLC0zQhme1sMbiQfQoCG9l/3zM77H2Vmd/ce7+P8y1QPi9Iw4sMPh50N25CS4r/lstdfVUqnvlFmVb5RfH57E1rd5TWG5nNpTY3yUFB4a8yqZveaeyb+e5xyc7DiSVC7U3qJqkMfdMkCUtcLL4uRN3xS3vDXn78hlb8XYVT8yfDX/4T2MWv/YmIa/CcJOz4NnBQbtoepGePjsSdSGclQVm1qvtT90GW/pVrISZ6BsszrIdAkildECIJJjk8RW/cvtfUyk99s9+e9/u3occV9T2PS+zwA" -O 12
```

Standard input: (use `cmd.exe /c` if running on PowerShell)
```
dotnet run --project HuffmanJK -- decode --stdin -O 12 < encoded.bin
```
