DOCS_OUT_DIR=docs

GEN_CMD="xmldoc2md"

generate-docs:
	docfx metadata   
	docfx build

.PHONY: generate-docs docker docker-rebuild

docker-rebuild:
	docker-compose down --rmi all -v
	docker-compose up --build -d

docker:
	docker-compose up --build
