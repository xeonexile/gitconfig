NUGET_SOURCES = -s https://api.nuget.org/v3/index.json
SEMVER_REGEX=^(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)\.(0|[1-9][0-9]*)(\-[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?(\+[0-9A-Za-z-]+(\.[0-9A-Za-z-]+)*)?
# Colors
RED = \033[0;32m
NC  = \033[0m

# SemVer
def_ver := 1.0.0
ver := $(or $(shell git describe --tags --abbrev=0),${def_ver})
pre ?= $(shell git branch | grep \* | cut -d ' ' -f2)
build ?= $(shell git rev-parse HEAD)
version ?= ${ver}$(shell echo -${pre})

FLAGS = -c Release /p:Version=${version}
OUT = $(shell pwd)/app

.PHONY: help # Show this help screen
help:
	@grep '^.PHONY: .* #' .common.mk | sed 's/\.PHONY: \(.*\) # \(.*\)/\1 :\2/' | expand -t20
	@grep '^.PHONY: .* #' Makefile | sed 's/\.PHONY: \(.*\) # \(.*\)/\1 :\2/' | expand -t20

.PHONY: all # clean deps test build
all : clean deps build

.PHONY: version # Display and check current version
version:

	@printf "${RED}[VERSION]${NC}\n"
	@printf "${RED}${version}${NC}\n"
	@if [[ ${version} =~ ${SEMVER_REGEX} ]]; then\
        echo "making v${version}";\
	else\
		echo "FAIL: invalid version";\
	fi

	@echo ${version}
	
.PHONY: clean # Clean up all build artifacts
clean:
	@printf "${RED}[CLEAN]${NC}\n"
	rm -rf ${OUT}
	dotnet clean

.PHONY: deps # Restore all dependencies prior to build
deps:
	@printf "${RED}[DEPS]${NC}\n"
	dotnet restore ${NUGET_SOURCES}

.PHONY: test # Run tests
test: deps
	@printf "${RED}[TEST]${NC}\n"
	dotnet test /p:CollectCoverage=true --filter "Category!=integration"

.PHONY: build # Build output artifact
build: version deps
	@printf "${RED}[BUILD]${NC}\n"
	dotnet publish ${FLAGS} -o ${OUT}
