// A generated module for Lexis functions
//
// This module has been generated via dagger init and serves as a reference to
// basic module structure as you get started with Dagger.
//
// Two functions have been pre-created. You can modify, delete, or add to them,
// as needed. They demonstrate usage of arguments and return types using simple
// echo and grep commands. The functions can be called from the dagger CLI or
// from one of the SDKs.
//
// The first line in this comment block is a short description line and the
// rest is a long description with more detail on the module's purpose or usage,
// if appropriate. All modules should have a short description.

package main

import (
	"context"
	"dagger/lexis/internal/dagger"
)

type Lexis struct{}

// Returns a container that echoes whatever string argument is provided
func (m *Lexis) ContainerEcho(stringArg string) *Container {
	return dag.Container().From("alpine:latest").WithExec([]string{"echo", stringArg})
}

// Returns lines that match a pattern in the files of the provided Directory
func (m *Lexis) GrepDir(ctx context.Context, directoryArg *Directory, pattern string) (string, error) {
	return dag.Container().
		From("alpine:latest").
		WithMountedDirectory("/mnt", directoryArg).
		WithWorkdir("/mnt").
		WithExec([]string{"grep", "-R", pattern, "."}).
		Stdout(ctx)
}

func (m *Lexis) BuildCSharpImage(directory *dagger.Directory) *dagger.Container {
	return dag.
		Container().
		From("mcr.microsoft.com/dotnet/sdk:8.0").
		WithMountedDirectory("/app", directory).
		WithWorkdir("/app").
		WithExec([]string{"dotnet", "build", "-c", "Release"}).
		WithExec([]string{
			"dotnet",
			"publish",
			"-p",
			"ContainerArchiveOutputPath=/app/images/",
			"/t:PublishContainer",
		})
}
