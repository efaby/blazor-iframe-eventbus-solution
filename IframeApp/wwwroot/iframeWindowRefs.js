export function getParentWindow(){ return window.parent; }

import { setIframe } from "./iframeBusInterop.js";

export function registerIframe(iframe) {
  setIframe(iframe);
}
