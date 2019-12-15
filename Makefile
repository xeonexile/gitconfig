SHELL := /bin/bash
include .common.mk

.PHONY: pack # Publish packages to repository
pack: build
	@printf "${RED}[PACK]${NC}\n"
	@dotnet pack ./ExileLab.Extensions.Configuration/ExileLab.Extensions.Configuration.csproj /p:Version=${version} -o ${OUT}