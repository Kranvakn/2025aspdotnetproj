/**
 * 메시지 박스
 * @param {*} title
 * @param {*} message
 * @param {*} onConfirm
 * @param {boolean} [enableCancel=true]
 */
function showMessageBox(title, message, onConfirm, enableCancel = true) {
  const overlay = document.createElement("div");
  overlay.className = "overlay";

  const dialog = document.createElement("div");
  dialog.className = "messageBox";
  dialog.innerHTML = `
        <h2>${title}</h2>
        <p>${message}</p>
        <button class="button" id="confirmBtn">확인</button>
        ${
          enableCancel ? '<button class="cancelButton" onclick="this.closest(\'.overlay\').remove()">취소</button>' : ""
        }
    `;

  overlay.appendChild(dialog);
  document.body.appendChild(overlay);

  document.getElementById("confirmBtn").onclick = () => {
    overlay.remove();
    if (typeof onConfirm === "function") onConfirm();
  };
}
