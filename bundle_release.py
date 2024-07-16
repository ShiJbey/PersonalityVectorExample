#!/usr/bin/python3

"""Release Bundler

This script assists with creating proper bundles for release

"""
import pathlib
import shutil
import subprocess
import xml.etree.ElementTree as ET

PACKAGE_NAME = "PersonalityVectorExample"

PROJECT_ROOT = pathlib.Path(__file__).parent

BUILD_DIR = (
    PROJECT_ROOT
    / "src"
    / "PersonalityVectorExample"
    / "bin"
    / "Release"
    / "netstandard2.1"
)

RELEASE_DIR = PROJECT_ROOT / "src" / "PersonalityVectorExample" / "bin" / "Release"

LICENSE_PATH = PROJECT_ROOT / "LICENSE.md"

README_PATH = PROJECT_ROOT / "README.md"

CSPROJ_PATH = (
    PROJECT_ROOT
    / "src"
    / "PersonalityVectorExample"
    / "PersonalityVectorExample.csproj"
)

OUTPUT_DIR = pathlib.Path("dist")


def get_project_version() -> str:
    """Read the project version number from the csproj file."""
    tree = ET.parse(CSPROJ_PATH)

    root = tree.getroot()

    try:
        version_elem = root.findall(".//Version")[0]
    except IndexError as exc:
        raise RuntimeError(
            f"Could not find <Version> element in: {CSPROJ_PATH}."
        ) from exc

    version_text = version_elem.text

    if not isinstance(version_text, str):
        raise TypeError(f"Version element in '{CSPROJ_PATH}' missing inner text.")

    return version_text


def main():
    """The main entry point for the script."""

    # Clean out the previous release
    if RELEASE_DIR.exists():
        shutil.rmtree(str(RELEASE_DIR))

    # Create a new build
    try:
        subprocess.run(["dotnet", "build", "--configuration", "Release"], check=True)
    except subprocess.CalledProcessError:
        print("An error occurred during build")
        return

    # Copy the license and readme to the built distribution
    shutil.copyfile(LICENSE_PATH, RELEASE_DIR / "LICENSE.md")
    shutil.copyfile(README_PATH, RELEASE_DIR / "README.md")

    # Zip the build directory
    project_version = get_project_version()

    shutil.make_archive(
        str(OUTPUT_DIR / f"{PACKAGE_NAME}_{project_version}"), "zip", RELEASE_DIR
    )


if __name__ == "__main__":
    main()
