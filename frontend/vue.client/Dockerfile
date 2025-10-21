FROM node:18-alpine AS build
WORKDIR /app

ARG VUE_APP_API_BASE_URL=""
ENV VUE_APP_API_BASE_URL=${VUE_APP_API_BASE_URL}

# Копируем только package.json и lock-файл чтобы максимально кэшировать слой установки
COPY package.json package-lock.json ./

# Устанавливаем зависимости строго по lock-файлу
RUN npm ci --legacy-peer-deps

# Копируем остальной код и собираем
COPY . .
RUN npm run build --if-present

FROM nginx:stable-alpine
COPY --from=build /app/dist /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]