(function () {
    const providerToggle = document.getElementById('ai-provider-toggle');
    const providerInput = document.getElementById('ai-provider-input');
    const userGuidInput = document.getElementById('ai-user-guid-input');
    const promptInput = document.getElementById('ai-prompt-input');
    const chatLog = document.getElementById('ai-chat-log');
    const form = document.getElementById('ai-prompt-form');

    document.querySelectorAll('#ai-prompt-form .dropdown-item').forEach(function (item) {
        item.addEventListener('click', function (e) {
            e.preventDefault();
            const provider = item.getAttribute('data-provider');
            providerToggle.textContent = provider;
            providerInput.value = provider;
        });
    });

    function appendMessage(role, text) {
        const entry = document.createElement('div');
        entry.textContent = `${role}: ${text}`;
        chatLog.appendChild(entry);
    }

    promptInput.addEventListener('keydown', async function (e) {
        if (e.key !== 'Enter') {
            return;
        }

        e.preventDefault();

        const prompt = promptInput.value.trim();
        if (!prompt) {
            return;
        }

        const provider = providerInput.value;
        const userGuid = userGuidInput.value;
        const token = form.querySelector('input[name="__RequestVerificationToken"]').value;

        appendMessage('You', prompt);
        promptInput.value = '';
        promptInput.disabled = true;

        try {
            const response = await fetch('?handler=Send', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': token
                },
                body: JSON.stringify({ provider, prompt, userGuid })
            });

            if (!response.ok) {
                appendMessage('Error', `Request failed (${response.status}).`);
                return;
            }

            const data = await response.json();
            appendMessage(data.provider, data.response);
        } catch (err) {
            appendMessage('Error', 'Unable to reach the server.');
        } finally {
            promptInput.disabled = false;
            promptInput.focus();
        }
    });
})();
