# CLI Parser Design #

## Concepts ##

### Commands ###

_Commands_ are actions that are supported by the application command line.

For example, in the command line `git checkout -b iss53`, the commands here are `git` and `checkout`.


### Root ###

The _root_ of a command line is the first listed command (generally the application/executable name). 

For example, in the command line `git checkout -b iss53`, the root command is `git`.


### Subcommand ###

If you’ve got a tool that’s sufficiently complex, you can reduce its complexity by making a set of subcommands. If you have several tools that are very closely related, you can make them easier to use and discover by combining them into a single command (for example, RCS vs. Git).

_Subcommands_ are commands that comes after the root command. Each subcommand may have a hierarchy of subcommands of its own.

For example, in the command line `git checkout -b iss53`, the subcommand is `checkout`.

### Flags ###

_Flags_ are named parameters, denoted with either a hyphen and a single-letter name (`-r`) or a double hyphen and a multiple-letter name (`--recursive`). They may or may not also include a user-specified value (`--file foo.txt`, or `--file=foo.txt`). The order of flags, generally speaking, does not affect program semantics.


For example, in the command line `git checkout -b iss53`, the flag is `-b`.

### Arguments ###

_Arguments_, or _args_, are positional parameters to a command. For example, the file paths you provide to `cp` are args. The order of args is often important: `cp foo bar` means something different from `cp bar foo`.

Arguments may be optional or required. 

For example, in the command line `git checkout -b iss53`, the argument is `iss53`.

## Implementation ##

How to build this?
Initially, I wanted to use a fluent interface/builder pattern to allow users to build up a CLI. A problem arises when we want to configure a CLI parser to handle a command hierarchy like:

```
dotnet
    add
        package
        reference
    remove
        package
        reference
    etc.
```
A fluent interface would get pretty messy here, so it will probably be easier to forgo the builder pattern in favor of either an object that needs to be configured, or a configuration object that can be parsed into a parser.

Javascript/TypeScript, due to the ease of creating free-form json objects, leans toward the latter (ex: [oclif](https://oclif.io/)).

Java (through [picocli](https://picocli.info/)) uses lots of decorators and interface implementations.

In C# we could use attributes to decorate classes that define `commands` and `subcommands`, and then a class to first read the configuration attributes, and then build a parser from that.  
This would very likely get complicated quickly for deep implementations like `dotnet`, with many flags.

The problem with that is that we cannot restrict attributes to use on a _specific_ type. This would have been ideal.

So maybe just a `CLIConfiguration` class which defines all the commands, subcommands, flags, and arguments.





[picocli]: https://picocli.info/
[clig-dev]: https://clig.dev
[docopt]: http://docopt.org/