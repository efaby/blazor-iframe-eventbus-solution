namespace Shared.Messages;

public sealed record LinkDto(string Text, string Href);

public sealed record ReadyPayload(string App, string Version);
public sealed record LinksUpdate(IReadOnlyList<LinkDto> Links);
public sealed record LinkClicked(string Href);
public sealed record NavGo(string Href, bool ForceLoad = false);
public sealed record NavRequest(string Href);
