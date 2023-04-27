# Blue Sector

Virtual Reality app for exploring blue sector workplaces and professions.

[Check out the docs!](https://github.com/vr4vet/Blue-Sector/wiki)

[![License](https://img.shields.io/badge/License-MIT-blue.svg?style=flat-square)](https://opensource.org/licenses/MIT)
[![Build](https://img.shields.io/github/actions/workflow/status/vr4vet/Blue-Sector/buid.yml?style=flat-square)](https://github.com/vr4vet/Blue-Sector/actions/workflows/conventionalcommits.yml)

## Screenshots

## Getting started

1. Ensure Unity `2021.3.5f1` is installed
1. `git clone git@github.com:vr4vet/Blue-Sector.git`
1. Obtain and download [BNG framework](BNG) and put it in the `Assets/` folder
1. Obtain FishMerd component and put it in the `Assets/FishFeeding/Components/`
   folder

## Documentation

Check out our [Wiki](https://github.com/vr4vet/Blue-Sector/wiki) to find more
complete documentation.

## Contributing

Check out our
[CONRIBUTING](https://github.com/vr4vet/Blue-Sector/CONTRIBUTING.md) file to see
how you can contribute

## Development

### Standards

This project strives to follow the
[conventional commits](https://www.conventionalcommits.org/en/v1.0.0/) standard
for commit messages. In an effort to make this specification more convenient to
follow, we include a gitmessage template.

To use this template, ensure you are in the root of the project and run the
command:

`git config commit.template .gitmessage`

This will set your local gitmessage template to the included file. If the -m
flag is used when issuing a commit the template will be ignored. It is also
worth mentioning that the project implements a commit message linter, which will
check your commit messages against the conventional commits standard when you
push to, or create a pull request against, the main or develop branches.

### Git LFS

Given that this is a unity project, it includes a lot of large texture, model,
and general media files. To speed up pushing, pulling, and development - as well
as saving storage - we utilise git LFS. Please ensure that you install the
[git LFS extension][lfs-install], and initiate it in your local repository with:

```sh
git lfs install
```

We have experienced reports of issues with LFS in the project.
If you get an error resembling:

"Found x files that should have been pointers,
but werent"

run these commands in order:

```sh
git lfs uninstall
git reset --hard
git lfs install
git lfs pull
```

For a more complete explanation of LFS, see [here][lfs].

[lfs]: https://git-lfs.com/
[lfs-install]: https://github.com/git-lfs/git-lfs#installing
[conventional-commits]: https://www.conventionalcommits.org/en/v1.0.0/
[BNG]: https://assetstore.unity.com/packages/templates/systems/vr-interaction-framework-161066
