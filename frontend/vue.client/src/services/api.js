import axios from 'axios';

const api = axios.create({
    baseURL: process.env.VUE_APP_API_BASE_URL || '',
    headers: { 'Content-Type': 'application/json' }
});
// const api = axios.create({
//     baseURL: 'https://localhost:9000/',
//     headers: { 'Content-Type': 'application/json' }
// });

export async function fetchLoans({ pageIndex = 1, pageSize = 10, status, minAmount, maxAmount, minTerm, maxTerm } = {}) {
        console.log(api.baseURL);
        console.log(process.env);
    const params = { pageIndex, pageSize };
    if (status !== undefined && status !== null) params.isPublished = status;
    if (minAmount !== undefined && minAmount !== null) params.minAmount = minAmount;
    if (maxAmount !== undefined && maxAmount !== null) params.maxAmount = maxAmount;
    if (minTerm !== undefined && minTerm !== null) params.minTerm = minTerm;
    if (maxTerm !== undefined && maxTerm !== null) params.maxTerm = maxTerm;
    const res = await api.get('/api/loans/filter', { params });
    return res.data;
}

export async function createLoan(payload) {
    const res = await api.post('/api/loans', payload);
    return res.data;
}

export async function toggleLoanPublish(loanId, publish) {
    const res = await api.put(`/api/loans`, { Id: loanId, IsPublished: publish });
    return res.data;
}

export default api;