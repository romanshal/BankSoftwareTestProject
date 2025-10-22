<template>
    <div>
        <h2>Loans list</h2>

        <LoanFilters v-model="filters" :disabled="loading" @apply="onFiltersApply" @reset="onFiltersReset" />

        <table class="loans-table">
            <thead>
                <tr>
                    <th>Number</th>
                    <th>Sum</th>
                    <th>Term (month)</th>
                    <th>Interest rate (%)</th>
                    <th>Status</th>
                    <th>Created at</th>
                    <th>Modified at</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="loan in loans" :key="loan.id">
                    <td>{{ loan.number }}</td>
                    <td>{{ loan.amount }}</td>
                    <td>{{ loan.termValue }}</td>
                    <td>{{ loan.interestValue }}</td>
                    <td>{{ loan.status ? 'Publish' : 'Unpublish' }}</td>
                    <td>{{ formatDate(loan.createdAt) }}</td>
                    <td>{{ formatDate(loan.modifiedAt) }}</td>
                    <td>
                        <button @click="togglePublish(loan)" :disabled="togglingId === loan.id || loading">
                            {{ loan.status ? 'Unpublish' : 'Publish' }}
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>

        <Paginator :rows="pageSize" :totalRecords="totalCount" :rowsPerPageOptions="[5, 10, 20, 30]"
            @page="onPageChange" />
    </div>
</template>

<script>
import { ref, onMounted } from 'vue';
import { fetchLoans, toggleLoanPublish } from '../services/api';
import LoanFilters from '../components/LoanFilters.vue';
import Paginator from 'primevue/paginator';

export default {
    name: 'LoanList',
    components: { Paginator, LoanFilters },
    setup() {
        const loans = ref([]);
        const pageIndex = ref(1); // 1-based index used by backend
        const pageSize = ref(5);
        const pagesCount = ref(1);
        const totalCount = ref(0);
        const togglingId = ref(null);
        const loading = ref(false);

        const filters = ref({
            status: null,
            minAmount: null,
            maxAmount: null,
            minTerm: null,
            maxTerm: null
        });

        async function load() {
            loading.value = true;

            const params = {
                pageIndex: pageIndex.value,
                pageSize: pageSize.value,
                status: filters.value.status,
                minAmount: filters.value.minAmount,
                maxAmount: filters.value.maxAmount,
                minTerm: filters.value.minTerm,
                maxTerm: filters.value.maxTerm
            };

            try {
                const data = await fetchLoans(params);

                loans.value = data.items || [];
                pagesCount.value = data.pagesCount || 1;
                totalCount.value = data.totalCount || 0;
                pageIndex.value = data.pageIndex || pageIndex.value;
                pageSize.value = data.pageSize || pageSize.value;
            } catch (err) {
                console.error('Failed to load loans', err);
                loans.value = [];
                pagesCount.value = 1;
                totalCount.value = 0;
            } finally {
                loading.value = false;
            }
        }

        function onFiltersApply() {
            pageIndex.value = 1;
            load();
        }

        function onFiltersReset() {
            pageIndex.value = 1;
            load();
        }

        function onPageChange(event) {
            const newPage = (typeof event.page === 'number') ? event.page + 1 : 1;
            const newRows = event.rows || pageSize.value;

            // Only reload if something changed
            if (newPage === pageIndex.value && newRows === pageSize.value) return;

            pageIndex.value = newPage;
            pageSize.value = newRows;

            load();
        }

        async function togglePublish(loan) {
            togglingId.value = loan.id;

            try {

                await toggleLoanPublish(loan.id, !loan.status);
                await load();
            } catch (err) {
                console.error('Toggle publish failed', err);
            } finally {
                togglingId.value = null;
            }
        }

        function formatDate(d) {
            if (!d) return '';
            const dt = new Date(d);
            return dt.toLocaleString();
        }

        onMounted(load);

        return {
            loans,
            pageIndex,
            pageSize,
            pagesCount,
            totalCount,
            filters,
            onFiltersApply,
            onFiltersReset,
            onPageChange,
            togglePublish,
            togglingId,
            formatDate,
            loading
        };
    }
};
</script>

<style scoped>
.loans-table {
    --lt-bg: #ffffff;
    --lt-head-bg: linear-gradient(180deg, #fbfeff, #f3f8fb);
    --lt-row-alt: #fbfdfe;
    --lt-border: #e6edf3;
    --lt-text: #0f172a;
    --lt-muted: #6b7280;
    --lt-accent: #2563eb;
    --lt-accent-600: #1e40af;
    --lt-radius: 10px;
    --lt-shadow: 0 6px 18px rgba(16, 24, 40, 0.06);
    width: 100%;
    border-collapse: separate;
    border-spacing: 0;
    background: var(--lt-bg);
    border-radius: var(--lt-radius);
    overflow: hidden;
    box-shadow: var(--lt-shadow);
    font-family: Inter, system-ui, -apple-system, "Segoe UI", Roboto, Arial;
    font-size: 0.95rem;
}

/* Заголовок таблицы */
.loans-table thead th {
    background: var(--lt-head-bg);
    color: var(--lt-muted);
    text-align: left;
    font-weight: 600;
    padding: 12px 16px;
    border-bottom: 1px solid var(--lt-border);
    font-size: 0.86rem;
    vertical-align: middle;
    letter-spacing: 0.2px;
}

/* Ячейки тела */
.loans-table tbody td {
    padding: 12px 16px;
    border-bottom: 1px solid var(--lt-border);
    vertical-align: middle;
    color: var(--lt-text);
    background: transparent;
}

/* Чёткая ширина для числовых колонок */
.loans-table td:nth-child(1),
.loans-table td:nth-child(3),
.loans-table td:nth-child(4) {
    white-space: nowrap;
    width: 10%;
}

/* Зебра-строки и hover-эффект */
.loans-table tbody tr:nth-child(even) td {
    background: var(--lt-row-alt);
}

.loans-table tbody tr:hover td {
    background: linear-gradient(90deg, rgba(245, 250, 255, 0.7), rgba(255, 255, 255, 0.95));
    transform: translateY(-1px);
    transition: background 180ms cubic-bezier(.2, .9, .3, 1), transform 180ms cubic-bezier(.2, .9, .3, 1);
    box-shadow: 0 4px 10px rgba(16, 24, 40, 0.04);
}

/* Статус-бейджи (встраиваемые) */
.loans-table .status {
    display: inline-block;
    padding: 6px 10px;
    border-radius: 999px;
    font-weight: 600;
    font-size: 0.80rem;
}

.loans-table .status.publish {
    color: #064e3b;
    background: #ecfdf5;
    border: 1px solid rgba(16, 185, 129, 0.06);
}

.loans-table .status.unpublish {
    color: #7c2d12;
    background: #fff7ed;
    border: 1px solid rgba(249, 115, 22, 0.06);
}

/* Кнопки в действиях — вид внутри таблицы */
.loans-table button {
    appearance: none;
    border: none;
    cursor: pointer;
    padding: 8px 12px;
    border-radius: 6px;
    font-weight: 600;
    font-size: 0.9rem;
    color: #fff;
    background: linear-gradient(180deg, var(--lt-accent), var(--lt-accent-600));
    box-shadow: 0 6px 12px rgba(37, 99, 235, 0.12);
    transition: transform 180ms cubic-bezier(.2, .9, .3, 1), box-shadow 180ms cubic-bezier(.2, .9, .3, 1), opacity 180ms;
}

.loans-table button:disabled {
    opacity: 0.56;
    cursor: not-allowed;
    background: linear-gradient(180deg, #cbd5e1, #9ca3af);
    box-shadow: none;
}

/* Адаптив: карточный вид на узких экранах */
@media (max-width: 880px) {
    .loans-table thead {
        display: none;
    }

    .loans-table,
    .loans-table tbody,
    .loans-table tr,
    .loans-table td {
        display: block;
        width: 100%;
    }

    .loans-table tr {
        margin-bottom: 12px;
        border-radius: 8px;
        padding: 10px;
        background: linear-gradient(180deg, #fff, #fbfeff);
        box-shadow: 0 4px 10px rgba(16, 24, 40, 0.04);
    }

    .loans-table td {
        padding: 8px 12px;
        border-bottom: 0;
    }

    .loans-table td::before {
        content: attr(data-label);
        display: inline-block;
        width: 40%;
        color: var(--lt-muted);
        font-weight: 600;
        margin-right: 8px;
    }
}
</style>