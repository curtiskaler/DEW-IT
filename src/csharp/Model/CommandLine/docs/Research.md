# Command Line Research #

Note that much of this content comes from outside sources:

[Command Line Interface Guidelines][clig-dev]  
[docopt: Command-line interface description language][docopt]  
[picocli - a mighty tiny command line interface][picocli]


## Concepts ##


### Commands ###

A `command` is an action that is supported by the application command line.

For example, in the command line

```
$ git checkout -b iss53
```

the commands here are `git` and `checkout`.


### Root Command ###

The `root` of a command line is a special type of command: specifically, it is the first listed command (generally the application/executable name). 

#### Example ####
In the command line

```
$ git checkout -b iss53
```

the root command here is `git`.


### Sub-Command ###

If you’ve got a tool that’s sufficiently complex, you can reduce its complexity by making a set of subcommands. If you have several tools that are very closely related, you can make them easier to use and discover by combining them into a single command (for example, RCS vs. Git).

They’re useful for sharing stuff—global flags, help text, configuration, storage mechanisms.

**Be consistent across subcommands**. Use the same flag names for the same things, have similar output formatting, etc.

**Use consistent names for multiple levels of subcommand**. If a complex piece of software has lots of objects and operations that can be performed on those objects, it is a common pattern to use two levels of subcommand for this, where one is a noun and one is a verb. For example, docker container create. Be consistent with the verbs you use across different types of objects.

Either `noun verb` or `verb noun` ordering works, but `noun verb` seems to be more common.

Further reading: [User experience, CLIs, and breaking the world, by John Starich.](https://uxdesign.cc/user-experience-clis-and-breaking-the-world-baed8709244f)

**Don’t have ambiguous or similarly-named commands.** For example, having two subcommands called “update” and “upgrade” is quite confusing. You might want to use different words, or disambiguate with extra words.

A `subcommand` is any command that comes after the root command. Each subcommand may have a hierarchy of subcommands of its own.

#### Example ####
 
In the command line

```
$ dotnet add [<PROJECT>] package <PACKAGE_NAME> [-f|--framework <FRAMEWORK>] [--interactive] [-n|--no-restore] [--package-directory <PACKAGE_DIRECTORY>] [--prerelease] [-s|--source <SOURCE>] [-v|--version <VERSION>]
```

the first subcommand is `add`, which includes (at least) one subcommand `package`.

### Arguments ###

_Arguments_, or _args_, are positional parameters to a command. For example, the file paths you provide to `cp` are args. The order of args is often important: `cp foo bar` means something different from `cp bar foo`.

**Prefer flags to args.** It’s a bit more typing, but it makes it much clearer what is going on. It also makes it easier to make changes to how you accept input in the future. Sometimes when using args, it’s impossible to add new input without breaking existing behavior or creating ambiguity.

Arguments may be optional or required. 

#### Example ####

In the command line

```
$ dotnet add [<PROJECT>] package <PACKAGE_NAME> [-f|--framework <FRAMEWORK>] [--interactive] [-n|--no-restore] [--package-directory <PACKAGE_DIRECTORY>] [--prerelease] [-s|--source <SOURCE>] [-v|--version <VERSION>]
```

the first argument is `PROJECT`, which is an optional argument for the `dotnet add` subcommand.

The second argument is `PACKAGE_NAME`, which is a required argument for the `dotnet add package` subcommand.

### Flags ###

_Flags_ are named parameters, denoted with either a hyphen and a single-letter name (`-r`) or a double hyphen and a multiple-letter name (`--recursive`). They may or may not also include a user-specified value (`--file foo.txt`, or `--file=foo.txt`). The order of flags, generally speaking, does not affect program semantics.

**Have full-length versions of all flags.** For example, have both `-h` and `--help`. Having the full version is useful in scripts where you want to be verbose and descriptive, and you don’t have to look up the meaning of flags everywhere.

**Only use one-letter flags for commonly used flags, particularly at the top-level when using subcommands.** That way you don’t “pollute” your namespace of short flags, forcing you to use convoluted letters and cases for flags you add in the future.

**Multiple arguments are fine for simple actions against multiple files.** For example, `rm file1.txt file2.txt file3.txt`. This also makes it work with globbing: `rm *.txt`.

**If you’ve got two or more arguments for different things, you’re probably doing something wrong.** The exception is a common, primary action, where the brevity is worth memorizing. For example, `cp <source> <destination>`.

**Use standard names for flags, if there is a standard.** If another commonly used command uses a flag name, it’s best to follow that existing pattern. That way, a user doesn’t have to remember two different options (and which command it applies to), and users can even guess an option without having to look at the help text.

Here’s a list of commonly used options:

  `-a`, `--all`: All. For example, ps, fetchmail.  
  `-d`, `--debug`: Show debugging output.  
  `-f`, `--force`: Force. For example, rm -f will force the removal of files, even if it thinks it does not have permission to do it. This is also useful for commands which are doing something destructive that usually require user confirmation, but you want to force it to do that destructive action in a script.  
  `--json`: Display JSON output. See the output section.  
  `-h`, `--help`: Help. This should only mean help. See the help section.  
  `--no-input`: See the interactivity section.  
  `-o`, `--output`: Output file. For example, sort, gcc.  
  `-p`, `--port`: Port. For example, psql, ssh.  
  `-q`, `--quiet`: Quiet. Display less output. This is particularly useful when displaying output for humans that you might want to hide when running in a script.  
  `-u`, `--user`: User. For example, ps, ssh.  
  `--version`: Version.  
  `-v`: This can often mean either verbose or version. You might want to use -d for verbose and this for version, or for nothing to avoid confusion.  

Make the default the right thing for most users. Making things configurable is good, but most users are not going to find the right flag and remember to use it all the time (or alias it). If it’s not the default, you’re making the experience worse for most of your users.

For example, `ls` has terse default output to optimize for scripts and other historical reasons, but if it were designed today, it would probably default to `ls -lhF`.

Prompt for user input. If a user doesn’t pass an argument or flag, prompt for it. (See also: [Interactivity](https://clig.dev/#interactivity))

Never require a prompt. Always provide a way of passing input with flags or arguments. If `stdin` is not an interactive terminal, skip prompting and just require those flags/args.

Confirm before doing anything dangerous. A common convention is to prompt for the user to type `y` or `yes` if running interactively, or requiring them to pass `-f` or `--force` otherwise.

“Dangerous” is a subjective term, and there are differing levels of danger:

- **Mild:** A small, local change such as deleting a file. You might want to prompt for confirmation, you might not. For example, if the user is explicitly running a command called something like “delete,” you probably don’t need to ask.  
- **Moderate:** A bigger local change like deleting a directory, a remote change like deleting a resource of some kind, or a complex bulk modification that can’t be easily undone. You usually want to prompt for confirmation here. Consider giving the user a way to “dry run” the operation so they can see what’ll happen before they commit to it.  
- **Severe:** Deleting something complex, like an entire remote application or server. You don’t just want to prompt for confirmation here—you want to make it hard to confirm by accident. Consider asking them to type something non-trivial such as the name of the thing they’re deleting. Let them alternatively pass a flag such as `--confirm="name-of-thing"`, so it’s still scriptable.

Consider whether there are non-obvious ways to accidentally destroy things. For example, imagine a situation where changing a number in a configuration file from 10 to 1 means that 9 things will be implicitly deleted—this should be considered a severe risk, and should be difficult to do by accident.

If input or output is a file, support - to read from stdin or write to stdout. This lets the output of another command be the input of your command and vice versa, without using a temporary file. For example, tar can extract files from stdin:

```
$ curl https://example.com/something.tar.gz | tar xvf -
```
If a flag can accept an optional value, allow a special word like “none.” For example, ssh -F takes an optional filename of an alternative ssh_config file, and ssh -F none runs SSH with no config file. Don’t just use a blank value—this can make it ambiguous whether arguments are flag values or arguments.

If possible, make arguments, flags and subcommands order-independent. A lot of CLIs, especially those with subcommands, have unspoken rules on where you can put various arguments. For example a command might have a --foo flag that only works if you put it before the subcommand:

```
$ mycmd --foo=1 subcmd
works

$ mycmd subcmd --foo=1
unknown flag: --foo
```

This can be very confusing for the user—especially given that one of the most common things users do when trying to get a command to work is to hit the up arrow to get the last invocation, stick another option on the end, and run it again. If possible, try to make both forms equivalent, although you might run up against the limitations of your argument parser.

Do not read secrets directly from flags. When a command accepts a secret, eg. via a `--password` argument, the argument value will leak the secret into `ps` output and potentially shell history. And, this sort of flag encourages the use of insecure environment variables for secrets.

Consider accepting sensitive data only via files, e.g. with a `--password-file` argument, or via stdin. A `--password-file` argument allows a secret to be passed in discreetly, in a wide variety of contexts.

(It’s possible to pass a file’s contents into an argument in Bash by using `--password $(< password.txt)`. This approach has the same security issue of leaking the file’s contents into the output of `ps`. It’s best avoided.)

#### Example ####
In the command line

```
$ dotnet add [<PROJECT>] package <PACKAGE_NAME> [-f|--framework <FRAMEWORK>] [--interactive] [-n|--no-restore] [--package-directory <PACKAGE_DIRECTORY>] [--prerelease] [-s|--source <SOURCE>] [-v|--version <VERSION>]
```

the flags for the `add package` sub-command include
* `-f|--framework`, which requires a `FRAMEWORK` argument
* `--interactive`
* `-n|--no-restore`
* `--package-directory`, which requires a `PACKAGE_DIRECTORY` argument
* `--prerelease`
* `-s|--source`, which requires a `SOURCE` argument
* `-v|--version`, which requires a `VERSION` argument

## Help ##

**Display help text** when passed no options, the `-h` flag, or the `--help` flag.

**Display a concise help text by default.** If you can, display help by default when myapp or myapp subcommand is run. Unless your program is very simple and does something obvious by default (e.g. ls), or your program reads input interactively (e.g. cat).

The concise help text should only include:

- A description of what your program does.
- One or two example invocations.
- Descriptions of flags, unless there are lots of them.
- An instruction to pass the --help flag for more information.

`jq` does this well. When you type `jq`, it displays an introductory description and an example, then prompts you to pass `jq --help` for the full listing of flags:

```
$ jq
jq - commandline JSON processor [version 1.6]

Usage:    jq [options] <jq filter> [file...]
    jq [options] --args <jq filter> [strings...]
    jq [options] --jsonargs <jq filter> [JSON_TEXTS...]

jq is a tool for processing JSON inputs, applying the given filter to
its JSON text inputs and producing the filter's results as JSON on
standard output.

The simplest filter is ., which copies jq's input to its output
unmodified (except for formatting, but note that IEEE754 is used
for number representation internally, with all that that implies).

For more advanced filters see the jq(1) manpage ("man jq")
and/or https://stedolan.github.io/jq

Example:

    $ echo '{"foo": 0}' | jq .
    {
        "foo": 0
    }

For a listing of options, use jq --help.
```

When passed a help flag, ignore any other flags and arguments that are passed—you should be able to add `-h` to the end of anything and it should show help. Don’t overload `-h`.

If your program is git-like, the following should also offer help.

```
$ myapp help
$ myapp help subcommand
$ myapp subcommand --help
$ myapp subcommand -h
```

**Provide a support path for feedback and issues.** A website or GitHub link in the top-level help text is common.

**In help text, link to the web version of the documentation**. If you have a specific page or anchor for a subcommand, link directly to that. This is particularly useful if there is more detailed documentation on the web, or further reading that might explain the behavior of something.

**Lead with examples.** Users tend to use examples over other forms of documentation, so show them first in the help page, particularly the common complex uses. If it helps explain what it’s doing and it isn’t too long, show the actual output too.

You can tell a story with a series of examples, building your way toward complex uses.

**If you’ve got loads of examples, put them somewhere else, in a cheat sheet command or a web page.** It’s useful to have exhaustive, advanced examples, but you don’t want to make your help text really long.

For more complex use cases, e.g. when integrating with another tool, it might be appropriate to write a fully-fledged tutorial.

**Display the most common flags and commands at the start of the help text.** It’s fine to have lots of flags, but if you’ve got some really common ones, display them first. For example, the Git command displays the commands for getting started and the most commonly used subcommands first:

```
$ git
usage: git [--version] [--help] [-C <path>] [-c <name>=<value>]
           [--exec-path[=<path>]] [--html-path] [--man-path] [--info-path]
           [-p | --paginate | -P | --no-pager] [--no-replace-objects] [--bare]
           [--git-dir=<path>] [--work-tree=<path>] [--namespace=<name>]
           <command> [<args>]

These are common Git commands used in various situations:

start a working area (see also: git help tutorial)
   clone      Clone a repository into a new directory
   init       Create an empty Git repository or reinitialize an existing one

work on the current change (see also: git help everyday)
   add        Add file contents to the index
   mv         Move or rename a file, a directory, or a symlink
   reset      Reset current HEAD to the specified state
   rm         Remove files from the working tree and from the index

examine the history and state (see also: git help revisions)
   bisect     Use binary search to find the commit that introduced a bug
   grep       Print lines matching a pattern
   log        Show commit logs
   show       Show various types of objects
   status     Show the working tree status
…
```

**Use formatting in your help text.** Bold headings make it much easier to scan. But, try to do it in a terminal-independent way so that your users aren’t staring down a wall of escape characters.


<html>
<pre>
<code>
<strong>$ heroku apps --help</strong>

list your apps

<strong>USAGE</strong>
  $ heroku apps

<strong>OPTIONS</strong>
  -A, --all          include apps in all teams
  -p, --personal     list apps in personal account when a default team is set
  -s, --space=space  filter by space
  -t, --team=team    team to use  
  --json             output in json format  

<strong>EXAMPLES</strong>
  $ heroku apps
  === My Apps
  example
  example2

  === Collaborated Apps
  theirapp   other@owner.name

<strong>COMMANDS</strong>
  apps:create     creates a new app
  apps:destroy    permanently destroy an app
  apps:errors     view app errors
  apps:favorites  list favorited apps
  apps:info       show detailed app information
  apps:join       add yourself to a team app
  apps:leave      remove yourself from a team app
  apps:lock       prevent team members from joining an app
  apps:open       open the app in a web browser
  apps:rename     rename an app
  apps:stacks     show the list of available stacks
  apps:transfer   transfer applications to another user or team
  apps:unlock     unlock an app so any team member can join
</code>
</pre>
</html>

Note: When `heroku apps --help` is piped through a pager, the command emits no escape characters.

If the user did something wrong and you can guess what they meant, suggest it. For example, brew update `jq` tells you that you should run `brew upgrade jq`.

You can ask if they want to run the suggested command, but don’t force it on them. For example:

```
$ heroku pss
 ›   Warning: pss is not a heroku command.
Did you mean ps? [y/n]:
```

Rather than suggesting the corrected syntax, you might be tempted to just run it for them, as if they’d typed it right in the first place. Sometimes this is the right thing to do, but not always.

Firstly, invalid input doesn’t necessarily imply a simple typo—it can often mean the user has made a logical mistake, or misused a shell variable. Assuming what they meant can be dangerous, especially if the resulting action modifies state.

Secondly, be aware that if you change what the user typed, they won’t learn the correct syntax. In effect, you’re ruling that the way they typed it is valid and correct, and you’re committing to supporting that indefinitely. Be intentional in making that decision, and document both syntaxes.

Further reading: [“Do What I Mean”](http://www.catb.org/~esr/jargon/html/D/DWIM.html)

**If your command is expecting to have something piped to it and stdin is an interactive terminal, display help immediately and quit.** This means it doesn’t just hang, like cat. Alternatively, you could print a log message to stderr.

For more information, check out [https://clig.dev][clig-dev].





[picocli]: https://picocli.info/
[clig-dev]: https://clig.dev
[docopt]: http://docopt.org/