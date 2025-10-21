<template>
    <section class="filters">
        <label>
            Status:
            <select v-model="local.status">
                <option :value="null">All</option>
                <option :value="true">Published</option>
                <option :value="false">Unpublished</option>
            </select>
        </label>

        <label>
            Sum from:
            <input type="number" v-model.number="local.minAmount" min="0" />
        </label>

        <label>
            to:
            <input type="number" v-model.number="local.maxAmount" min="0" />
        </label>

        <label>
            Term from (month):
            <input type="number" v-model.number="local.minTerm" min="0" />
        </label>

        <label>
            to (month):
            <input type="number" v-model.number="local.maxTerm" min="0" />
        </label>

        <button @click="onApply" :disabled="applying">Apply</button>
        <button @click="onReset" :disabled="applying">Reset</button>
    </section>
</template>

<script>
import { reactive, watch, computed } from 'vue';

export default {
    name: 'LoanFilters',
    props: {
        modelValue: {
            type: Object,
            default: () => ({ status: null, minAmount: null, maxAmount: null, minTerm: null, maxTerm: null })
        },
        disabled: {
            type: Boolean,
            default: false
        }
    },
    emits: ['update:modelValue', 'apply', 'reset'],
    setup(props, { emit }) {
        const local = reactive({
            status: props.modelValue?.status ?? null,
            minAmount: props.modelValue?.minAmount ?? null,
            maxAmount: props.modelValue?.maxAmount ?? null,
            minTerm: props.modelValue?.minTerm ?? null,
            maxTerm: props.modelValue?.maxTerm ?? null
        });

        watch(
            () => props.modelValue,
            (nv) => {
                local.status = nv?.status ?? null;
                local.minAmount = nv?.minAmount ?? null;
                local.maxAmount = nv?.maxAmount ?? null;
                local.minTerm = nv?.minTerm ?? null;
                local.maxTerm = nv?.maxTerm ?? null;
            },
            { deep: true, immediate: true }
        );

        watch(
            () => ({ ...local }),
            (nv) => {
                emit('update:modelValue', { ...nv });
            },
            { deep: true }
        );

        const applying = computed(() => props.disabled);

        function onApply() {
            emit('update:modelValue', { ...local });
            emit('apply');
        }

        function onReset() {
            local.status = null;
            local.minAmount = null;
            local.maxAmount = null;
            local.minTerm = null;
            local.maxTerm = null;
            emit('update:modelValue', { ...local });
            emit('reset');
        }

        return { local, onApply, onReset, applying };
    }
};
</script>

<style scoped>
/* Стили для фильтров */
.filters {
    display: flex;
    flex-wrap: wrap;
    gap: 12px;
    align-items: flex-end;
    padding: 12px;
    background: #ffffff;
    border: 1px solid #e6e9ef;
    border-radius: 8px;
    box-shadow: 0 1px 2px rgba(16, 24, 40, 0.04);
    font-family: Inter, ui-sans-serif, system-ui, -apple-system, "Segoe UI", Roboto, "Helvetica Neue", Arial;
    color: #0f172a;
    margin-bottom: 16px;
}

/* Каждый контрол объединён в компактный блок label */
.filters>label {
    display: flex;
    flex-direction: column;
    gap: 6px;
    min-width: 140px;
    max-width: 320px;
    flex: 1 1 160px;
    font-size: 13px;
    line-height: 1.2;
}

/* Ярлык текста (пометка сверху) */
.filters>label::first-line {
    font-weight: 600;
    color: #0b1226;
}

/* select и input — единый визуальный стиль */
.filters select,
.filters input[type="number"] {
    -webkit-appearance: none;
    appearance: none;
    width: 100%;
    box-sizing: border-box;
    padding: 8px 10px;
    border: 1px solid #cbd5e1;
    border-radius: 6px;
    background: #fff;
    font-size: 14px;
    color: #0f172a;
    transition: border-color 120ms ease, box-shadow 120ms ease;
}

/* focus state */
.filters select:focus,
.filters input[type="number"]:focus {
    outline: none;
    border-color: #2563eb;
    box-shadow: 0 0 0 3px rgba(37, 99, 235, 0.12);
}

/* кнопки */
.filters>button {
    padding: 9px 14px;
    border-radius: 8px;
    border: 1px solid transparent;
    background: #111827;
    color: #ffffff;
    font-weight: 600;
    cursor: pointer;
    transition: transform 80ms ease, background-color 120ms ease, box-shadow 120ms ease;
    min-width: 92px;
    flex: 0 0 auto;
}

/* вторичная кнопка (Reset) — визуально нейтральнее */
.filters>button:nth-of-type(2) {
    background: #f8fafc;
    color: #0b1226;
    border-color: #e6e9ef;
    box-shadow: none;
}

/* hover/active states */
.filters>button:not(:disabled):hover {
    transform: translateY(-1px);
    box-shadow: 0 6px 18px rgba(2, 6, 23, 0.08);
}

.filters>button:active {
    transform: translateY(0);
}

/* disabled */
.filters>button:disabled,
.filters select:disabled,
.filters input[type="number"]:disabled {
    opacity: 0.56;
    cursor: not-allowed;
    box-shadow: none;
}

/* Малые экраны: расположение в колонку */
@media (max-width: 720px) {
    .filters {
        gap: 10px;
    }

    .filters>label {
        flex: 1 1 100%;
        min-width: 0;
    }

    .filters>button {
        width: 48%;
    }
}

/* Дополнительные утилиты: компактный режим (если нужно) */
.filters.compact {
    gap: 8px;
    padding: 8px;
}

.filters.compact>label {
    gap: 4px;
    font-size: 12px;
}
</style>