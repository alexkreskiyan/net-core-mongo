include Makelib

PACKAGE = MongoSample

run:
	@dotnet exec ./src/$(PACKAGE)/bin/$(CONFIG)/netcoreapp1.1/$(PACKAGE).dll

pack:
	@$(MAKE) rebuild CONFIG=$(CONFIG)
	@$(call pack,$(PACKAGE))
	@echo "Done"

clear-local:
	@$(call clear-local,$(PACKAGE))

clear-server:
	$(call clear-server,$(PACKAGE))

push-local:
	@$(call push-local,$(PACKAGE))

push-server:
	@$(call push-server,$(PACKAGE))
