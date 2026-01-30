let allowedOrigins = [];
let dotnetRef = null;
let iframeWindow = null;

export function configure(origins) {
  allowedOrigins = origins ?? [];
}

export function registerIframe(iframeEl) {
  iframeWindow = iframeEl?.contentWindow ?? null;
}

export function start(ref) {
  dotnetRef = ref;

  console.log("Bus started", location.origin);

  window.addEventListener("message", (event) => {
    if (allowedOrigins.length && !allowedOrigins.includes(event.origin)) return;

    const data = event.data;
    if (!data || typeof data.type !== "string") return;

    dotnetRef.invokeMethodAsync(
      "ReceiveAsync",
      data.type,
      data.payload ?? null
    );
  });
}

export function sendToIframe(type, payload, targetOrigin) {
  console.log("Sending to iframe", type);
  if (!iframeWindow) throw "Iframe not registered";
  iframeWindow.postMessage({ type, payload }, targetOrigin);
}

export function sendToParent(type, payload, targetOrigin) {
  console.log("Sending to home", type);
  window.parent.postMessage({ type, payload }, targetOrigin);
}
