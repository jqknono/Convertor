
   

- [Features](#features)
- [Start](#start)
	- [In Command](#in-command)
	- [Direct Start](#direct-start)
- [Definition of Action](#definition-of-action)
- [Write a Initialization File](#write-a-initialization-file)
	- [Standard Style](#standard-style)
		- [Example](#example)
	- [Simple Style](#simple-style)
		- [Example](#example)
		- [Limitation](#limitation)
- [Appendix](#appendix)
	- [Other Mark](#other-mark)
	- [Regular Expression](#regular-expression)
	- [File Types can be handled](#file-types-can-be-handled)
	- [Escape Characters](#escape-characters)
	- [Avoid](#avoid)


   

# Features

* Replace (Replace): Batch replace the file name and file content string content. The string to be replaced is represented by regular expressions, and users need to know how to use regular expressions.
* Extract: Find a string that matches a certain pattern, convert it into the required format, and save it to a new file.

# Start

## In Command

Enter in cmd:
> path/to/convetor.exe –i path/to/input –o path/to/output –l path/to/log.txt –ini=path/to/inifile

* `-i`: Indicates the input file (folder), the path to the file or folder to convert. When the path is a folder, all files in the folder will be operated on. If not set, the default is the input folder in the same path as the program;
* `-o`: Indicates the output folder, where the converted files are stored. If not set, the default is the output folder in the same path as the input file (folder);  
* `-l`: Indicates to record the log, the modified place will be recorded. If not set, the default is the log.txt file in the same path as the program;
* `-ini`: Define the path of the operation file, describe the content type to be replaced (**Type**), the string to be replaced (**Target**), the string to be replaced with (**Desired* *). If not set, the default is all files with .ini suffix in the same path of the program. `-ini` can be a single file or folder path. When it is a folder, read all ini files in the folder and complete all operations. Users can combine different ini files by themselves and put them in the folder of the `-ini` path to complete all operations.

## Direct Start
`-i`: The default is the input folder in the same path as the program;  
`-o`: The default is the output folder in the same path as the input file (folder);  
`-l`: The default is the log.txt file in the same path as the program;  
`-ini`: The default is all files with .ini suffix in the same path of the program;

# Definition of Action
`[Action]`: Represents an action, applied to each file to be processed;  
`Type`: Indicates that the file name (*filename*) or file content (*filecontent*) to be processed, the file content is more complicated and needs to be processed separately. When **Type** is not written, the default is to process the file content;  
`Target`: Indicates the form of the string you want to replace, please refer to Regular Expression for the writing method;  
`Desired`: Indicates the string expression you want to replace with, in a few cases, you can refer to Regex's Group;  

# Write a Initialization File
Create a new text document, change the suffix to .ini, and double-click to edit.  
[Actions1] can be named arbitrarily, but cannot be repeated. Repeating it will invalidate other operations; for the convenience of future use, try to choose a name with practical significance, such as: [*replace A with B*].
The program will read multiple ini files, and the actions in them will be collected. If there are duplicate names (even if they are in different ini files), the program will only execute one of them and skip other actions with the same name. If the action is indeed the same, do not delete the duplicate action. Users can combine ini files according to their own needs.  
**Type** can only write *filename* (same as *name*) and *filecontent* (same as *content*) as needed.  
**Target** and **Desired** need to refer to regular expressions.  
A line starting with `;` indicates that this line is a comment and has no effect on the operation result.

## Standard Style
A standard Action notation includes `Action`, `Type`, `Target`, `Desired`.  

### Example
````
[Delete the “_G00[^.]*” in filename]  
Type=name  
Target="_G00[^.]*"  
Desired=""
````

## Simple Style
* With `MakeItSimple=True` is a simplified ini file.
* Actions in an ini can only be all canonical writing, or all simplified writing.
* In simplified notation, one line represents an action, and sometimes there are restrictions.
* `name` and `content` are reserved keywords for the program (there are other uses in the program when the writing is simplified. When the replacement or replaced statement contains the name or content string, the standard writing must be used. For example, the string to be replaced It contains name, and the actual content to be modified is the content. In this case, you need to use the standard writing method, write it in another ini file, and put it in the same directory.
* If the replacement or replaced statement contains spaces, only canonical statements can be used.
Simple statements that cannot be parsed will be prompted, so you can test with confidence.

### Example
````
MakeItSimple=True  
; Each of the following non-commented sentences is a valid Action.  
Replace the "_G00\d*" in file name with "".  
; When the replacement target is content, it can be omitted or not written,  
; Replace the "_G00\d*" in file content with "". Equivalent to Replace the "_G00\d*" with "".  
Change "A" in name to "B"  
name "B" to "C"  
content "C" "B"  
; The next operation is to replace all A's with B's in the file content  
"A" "B"
````

### Limitation
- ; change “content” in filename to “A”, can be parsed, but the parsed result is unknown.  
- ; change “name” in file content to “A”, can be parsed, but the parsing result is unknown.  
- ; "A" " ", the replacement content or the replaced content has spaces, and the parsing failed.  
- ; "A" "AB", the replacement content or the replaced content has spaces, and the parsing fails.

# Appendix
## Other Marks
* MakeItSimple
write in ini file
`MakeItSimple=Ture`
to use simplified notation.

* OperationType
write in ini file
`OperationType=Extract`
to use the Extract (`Extract`) mode, which by default is the Replace (`Replace`) mode.

* FileType
write in ini file  
````
[FileType]  
TextFile=.html|.htm
BinaryFile=.gif  
````
to add other file extensions to be processed.

## Regular Expression
URLs [regexr](http://regexr.com/) that can test Regular Expressions.

## File Types can be handled
There are two types of files: text files and binary files. Usually, a file that can be opened with Notepad without garbled characters is a text file. Pictures, videos, etc. are binary files. By default, the program can process the names and contents of text documents with three extensions of txt/xml/ditamap, as well as the names of jpg/png files. If the user has files with other suffixes to process, they can write in the ini file:  
````
[FileType]  
TextFile=.dita|.bbb|.ccc  
BinaryFile=.dd|.ee|.fff  
````
**FileType, TextFile, BinaryFile** are fixed expressions, .aaa represents a text file with a suffix of .aaa, and multiple suffixes are separated by "|". **TextFile** means text file, **BinaryFile** means binary file.

## Escape Characters
Escape characters can be used. When using escape characters, do not put double quotes around the values ​​of **Target** and **Desired**.
For example: `Desired=\r\n`  
The only escape characters that can be used are: \f, \n, \r, \t, \v, \r\n

## Avoid
The replacement logic of the program is to repeat the matching pattern until it fails to match. So if you want to replace **A** with **AB**, the intuition is to write the following ACtion
````
[This is a WRONG action]
Target="A"
Desired="AB"
````
This is wrong and will cause the program to stop inside an infinite loop. Because after **A** is replaced with **AB**, there is still **A** in it, the program will execute the replacement again to get **AAB**, and continue to replace it to get **AAAAAA... . ..B**, the program will loop infinitely here, please avoid writing such Action.
You can try the following forms of Action to avoid infinite loops.
````
[modify this on your need]
Target="A[^B]{1}"
Desired="AB"
````
