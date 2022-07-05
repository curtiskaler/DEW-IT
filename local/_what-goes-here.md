# local #

A folder for machine-specific configuration files or scripts. For example:

> **NOTE** that the gitignore will be updated to explicitly include specific folders like `CI` and `gitlab`, but will otherwise ignore all sub-folders.

| sub-folder | purpose | source-controlled? <br/>:white_check_mark: (yes) / (no) :x: |
| --- | --- | :-: |
| ./\* | Anything that all machines might need, but doesn't belong in the 'build' folder | :white_check_mark: |
| ./CI/\* | Anything that CI machines might need | :white_check_mark: |
| ./gitlab/\* | Anything that "gitlab" machines might need |:white_check_mark: |
| ./lp5-ckr1-dsa/\* | Anything that's only useful to my personal machine.<br />Things that I want organized with the project but not committed to source-control. | :x: |

