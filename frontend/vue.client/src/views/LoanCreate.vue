<template>
    <div class="create-loan">
        <h2>Create loan</h2>

        <form class="loan-form" @submit.prevent="onSubmit" novalidate>
            <div class="form-row">
                <label class="field-label" for="number">Number</label>
                <input id="number" class="field-input" v-model="form.number" type="text" />
                <div v-if="errors.number" class="field-error">{{ errors.number }}</div>
            </div>

            <div class="form-row">
                <label class="field-label" for="amount">Sum</label>
                <input id="amount" class="field-input" v-model.number="form.amount" type="number" min="0" />
                <div v-if="errors.amount" class="field-error">{{ errors.amount }}</div>
            </div>

            <div class="form-row">
                <label class="field-label" for="termValue">Term (month)</label>
                <input id="termValue" class="field-input" v-model.number="form.termValue" type="number" min="0" />
                <div v-if="errors.termValue" class="field-error">{{ errors.termValue }}</div>
            </div>

            <div class="form-row">
                <label class="field-label" for="interestValue">Interest rate (%)</label>
                <input id="interestValue" class="field-input" v-model.number="form.interestValue" type="number" min="0"
                    step="0.01" />
                <div v-if="errors.interestValue" class="field-error">{{ errors.interestValue }}</div>
            </div>

            <div class="form-actions">
                <button class="btn-primary" type="submit" :disabled="submitting">Create</button>
                <router-link class="btn-link" to="/">Back to list</router-link>
            </div>
        </form>
    </div>
</template>

<script>
import { reactive, ref } from 'vue';
import { createLoan } from '../services/api';
import { useRouter } from 'vue-router';
import { useToast } from 'primevue/usetoast';

export default {
    name: 'LoanCreate',
    setup() {
        const router = useRouter();
        const toast = useToast();
        const form = reactive({
            number: '',
            amount: null,
            termValue: null,
            interestValue: null
        });
        const errors = reactive({});
        const submitting = ref(false);

        function validate() {
            errors.number = form.number ? '' : 'Number is required';
            errors.amount = (!form.amount || form.amount <= 0) ? 'Sun must be greater than 0' : '';
            errors.termValue = (!form.termValue || form.termValue <= 0) ? 'Term must be greater than 0' : '';
            errors.interestValue = (!form.interestValue || form.interestValue <= 0 || form.interestValue > 100) ? 'Interest rate must be greater than 0 and less than 100' : '';
            return !(errors.number || errors.amount || errors.termValue || errors.interestValue);
        }

        async function onSubmit() {
            if (!validate()) return;

            submitting.value = true;

            try {
                const payload = {
                    number: form.number,
                    amount: form.amount,
                    termValue: form.termValue,
                    interestValue: form.interestValue
                };

                await createLoan(payload);

                router.push({ path: '/' });
            } catch (err) {
                toast.add({ severity: 'error', summary: 'Error', detail: err?.message || 'Unknown error', life: 5000 });
            } finally {
                submitting.value = false;
            }
        }

        return { form, errors, onSubmit, submitting };
    }
};
</script>

<style scoped>
.create-loan {
    --bg: #ffffff;
    --surface: #f8fafc;
    --text: #0f172a;
    --muted: #6b7280;
    --accent: #2563eb;
    --accent-600: #1e40af;
    --danger: #dc2626;
    --border: #e6edf3;
    --radius: 10px;
    --shadow: 0 8px 22px rgba(16, 24, 40, 0.06);
    --transition: 180ms cubic-bezier(.2, .9, .3, 1);
    display: block;
    background: linear-gradient(180deg, var(--surface), var(--bg));
    padding: 18px;
    border-radius: calc(var(--radius) + 2px);
    box-shadow: var(--shadow);
    color: var(--text);
    font-family: Inter, system-ui, -apple-system, "Segoe UI", Roboto, Arial;
}

/* Heading */
.create-loan>h2 {
    margin: 0 0 14px 0;
    font-size: 1.125rem;
    font-weight: 600;
    color: var(--text);
}

/* Form */
.loan-form {
    display: grid;
    grid-template-columns: 1fr;
    gap: 12px;
}

/* Form row */
.form-row {
    display: flex;
    flex-direction: column;
    gap: 6px;
    background: #fff;
    padding: 12px;
    border: 1px solid var(--border);
    border-radius: 8px;
    transition: box-shadow var(--transition), transform var(--transition);
}

.form-row:focus-within {
    box-shadow: 0 8px 18px rgba(37, 99, 235, 0.08);
    transform: translateY(-2px);
    border-color: rgba(37, 99, 235, 0.12);
}

/* Label */
.field-label {
    font-weight: 600;
    color: var(--muted);
    font-size: 0.9rem;
}

/* Input */
.field-input {
    -webkit-appearance: none;
    appearance: none;
    border: 1px solid #dbe7ef;
    background: linear-gradient(180deg, #ffffff, #fbfdff);
    padding: 10px 12px;
    border-radius: 8px;
    font-size: 0.95rem;
    color: var(--text);
    transition: border-color var(--transition), box-shadow var(--transition), transform var(--transition);
}

.field-input:focus {
    outline: none;
    border-color: var(--accent);
    box-shadow: 0 6px 16px rgba(37, 99, 235, 0.08);
}

/* Error */
.field-error {
    color: var(--danger);
    font-size: 0.85rem;
    margin-top: 6px;
    font-weight: 600;
}

/* Actions */
.form-actions {
    display: flex;
    gap: 12px;
    align-items: center;
    justify-content: flex-start;
    margin-top: 6px;
}

/* Primary button */
.btn-primary {
    appearance: none;
    border: none;
    cursor: pointer;
    padding: 10px 14px;
    border-radius: 8px;
    font-weight: 700;
    color: #fff;
    background: linear-gradient(180deg, var(--accent), var(--accent-600));
    box-shadow: 0 8px 20px rgba(37, 99, 235, 0.12);
    transition: transform var(--transition), box-shadow var(--transition), opacity var(--transition);
}

.btn-primary:hover:not(:disabled) {
    transform: translateY(-2px);
    box-shadow: 0 12px 26px rgba(30, 64, 175, 0.14);
}

.btn-primary:disabled {
    cursor: not-allowed;
    opacity: 0.6;
    background: linear-gradient(180deg, #cbd5e1, #9ca3af);
    box-shadow: none;
}

/* Link styled as secondary action */
.btn-link {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 8px 12px;
    border-radius: 8px;
    color: var(--accent-600);
    text-decoration: none;
    font-weight: 600;
    background: transparent;
    border: 1px solid transparent;
}

.btn-link:hover {
    text-decoration: underline;
}

/* Compact layout for narrow screens */
@media (max-width: 720px) {
    .loan-form {
        gap: 10px;
    }

    .form-actions {
        flex-direction: column;
        align-items: stretch;
    }

    .btn-link {
        justify-content: center;
    }
}
</style>