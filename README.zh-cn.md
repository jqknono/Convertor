<!-- TOC depthFrom:1 depthTo:6 withLinks:1 updateOnSave:1 orderedList:0 -->

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

<!-- /TOC -->

# Features

* 替换(Replace)：批量替换文件名和文件内容字符串内容。要替换的字符串使用正则表达式表示，使用者需要了解正则表达式的使用方法。
* 提取(Extract)：寻找符合一定模式的字符串，转换成需要的格式，存到新文件中。

# Start

## In Command

在cmd中输入:
> path/to/convetor.exe –i path/to/input –o path/to/output –l path/to/log.txt –ini=path/to/inifile

* `-i`: 表示输入文件（夹），要转换的文件或文件夹的路径。路径为文件夹时，将对文件夹内所有文件进行操作。未设置的情况下，默认是和程序相同路径下的input文件夹；
* `-o`: 表示输出文件夹，转换后的文件存储的地方。未设置的情况下，默认是和输入文件（夹）相同路径下的output文件夹；  
* `-l`: 表示记录日志，修改过的地方会记录下来。未设置的情况下，默认是和程序相同路径下的log.txt文件；
* `-ini`: 定义操作文件的路径，描述要替换的内容类型（**Type**）、要替换的字符串（**Target**）、要替换成的字符串（**Desired**）。未设置的情况下，默认是程序相同路径下的所有.ini后缀名 的文件。`-ini`可以是单个文件或文件夹路径，为文件夹时，读取文件夹中的所有ini文件，完成所有操作。用户可以自行组合不同的ini文件，放在`-ini`路径的文件夹中，完成所有操作。

## Direct Start
`-i`: 默认是和程序相同路径下的input文件夹;  
`-o`: 默认是和输入文件（夹）相同路径下的output文件夹;  
`-l`: 默认是和程序相同路径下的log.txt文件;  
`-ini`: 默认是程序相同路径下的所有.ini后缀名的文件;

# Definition of Action
`[Action]`: 表示一种动作，应用在要处理的每个文件上；  
`Type`: 表示要处理的是文件名（*filename*）或文件内容（*filecontent*），文件内容的情况复杂一些，要分开处理。不写**Type**时，默认为处理文件内容；  
`Target`: 表示想要替换走的字符串的形式，写法参考Regular Expression；  
`Desired`: 表示想要替换为的字符串表达形式，少数情况可以参考Regex的Group；  

# Write a Initialization File
新建文本文档，改后缀名为.ini，双击编辑。  
[Actions1]可以任意取名，但不能重复，重复会使其它操作失效；为方便以后的使用，应尽量取具有实际意义的名字，如：[*replace A with B*]。
程序会读取多个ini文件，其中的action都会被搜集，如有重复名（即使位于不同ini文件中也是重复），程序只会执行其中一个而跳过其它重名action。如果动作确实是一样的，不用删除重复的action。用户可以根据需要自行组合ini文件。  
**Type** 只能根据需要写*filename*（同*name*）、*filecontent*（同 *content*）。  
**Target** 及 **Desired** 的写法需要参考正则表达式。  
一行以`;`开始表示这一行是注释，对操作结果没有影响。

## Standard Style
一个标准Action的写法包括`Action`, `Type`, `Target`, `Desired`.  

### Example
```
[Delete the “_G00[^.]*” in filename]  
Type=name  
Target="_G00[^.]*"  
Desired=""
```

## Simple Style
* 有`MakeItSimple=True`的是简化写法的ini文件。
* 一个ini中的action只能全是规范写法，或全是简化写法。
* 简化写法中，一行代表一个action，有时会有限制。
* `name`和`content`为程序保留关键字（简化写法时在程序中有别的用途，替换或被替换语句中有name或content字符串时，必须使用规范写法。比如要替换的字符串中就含有name，而实际要修改的是content, 这种情况要使用规范写法，写在另一个ini文件里，放在同一个目录即可。
* 替换或被替换语句中含有空格的，也只能使用规范语句。
不能解析的简单语句会有提示，可放心测试。

### Example
```
MakeItSimple=True  
; 以下每一行非注释句子都是一个合法的Action。  
Replace the "_G00\d*" in file name with "".  
; 替换目标为content时可省略不写，  
; Replace the "_G00\d*" in file content with "".等同于Replace the "_G00\d*" with "".  
Change "A" in name to "B"  
name "B" to "C"  
content "C" "B"  
; 下行操作为将file content中的所有A替换为B  
"A"     "B"
```

### Limitation
- ; change  “content”  in filename to “A”, 可以解析，但解析结果不可知。  
- ; change  “name”  in file content to “A”, 可以解析，但解析结果不可知。  
- ; “A”            “      “,   	替换内容或被替换内容有空格， 解析失败。  
- ; “A”            “ A        B“,   	替换内容或被替换内容有空格， 解析失败。

# Appendix
## Other Mark
* MakeItSimple
在ini文件中写入
`MakeItSimple=Ture`
以使用简化写法。

* OperationType
在ini文件中写入
`OperationType=Extract`
以使用提取(`Extract`)模式，默认情况下是替换(`Replace`)模式。

* FileType
在ini文件中写入  
```
[FileType]  
TextFile=.html|.htm
BinaryFile=.gif  
```
来添加要处理的其它后缀名的文件。

## Regular Expression
可以测试Regular Expression正则表达式的网址[regexr](http://regexr.com/).

## File Types can be handled
文件分为文本文件和二进制文件两种。通常，可以用记事本（Notepad）打开并且没有乱码的文件就是文本文件。图片、视频之类的是二进制文件。程序默认可以处理txt/xml/ditamap三种扩展名的文本文档的名字和内容，及jpg/png文件的名字。如果用户有其它后缀名的文件要处理，可以在ini文件中写入：  
```
[FileType]  
TextFile=.dita|.bbb|.ccc  
BinaryFile=.dd|.ee|.fff  
```
**FileType, TextFile, BinaryFile** 是固定表达，.aaa表示后缀名为.aaa的文本文档，多个后缀名之间以”|”分隔。**TextFile**表示文本文件，**BinaryFile** 表示二进制文件。

## Escape Characters
可以使用转义字符，使用转义字符时，不要在**Target**和**Desired**的值上加双引号。
例如：`Desired=\r\n`  
可以使用的转义字符只有：\f, \n, \r, \t, \v, \r\n

## Avoid
程序的替换逻辑是重复匹配模式，直到不能匹配。因此如果你想将**A**换成**AB**，直觉是写如下的ACtion
```
[This is a WRONG action]
Target="A"
Desired="AB"
```
这是错误的，将导致程序停止在无限循环内。因为**A**替换成**AB**后，里面仍然有**A**，程序将会再次执行替换得到**AAB**，并一直替换下去，得到**AAAAAA... ...B**,程序将在这里无限循环，请避免写这样的Action。
可以尝试如下形式的Action，避免死循环。
```
[modify this on your need]
Target="A[^B]{1}"
Desired="AB"
```
