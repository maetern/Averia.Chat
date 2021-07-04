
<template>
    <div id="app" class="form-group">

        <label>Логин</label>
        <input type="text"
               v-model="login"
               placeholder="Введите логин"
               class="form-control" />

        <button v-if="!signInComplete" v-on:click="authUser()" class="btn btn-primary">Авторизация</button>

        <div v-if="signInComplete">
            <label>Сообщение</label>
            <input type="text"
                   v-model="message"
                   placeholder="Введите сообщение"
                   class="form-control" /><br />

            <button v-on:click="sendMessage()" class="btn btn-primary">
                Сообщение
            </button>

            <ul v-if="messages">
                <li v-for="(message, index) in messages" :key="index">
                    {{ message.Date }} {{ message.Author }}: {{ message.Text }}
                </li>
            </ul>

        </div>

    </div>

</template>

<script>
    export default {
        name: "App",
        data: function () {
            return {
                connection: null,
                signInComplete: false,
                login: "",
                message: "",
                messages: [],
            };
        },
        methods: {
            authUser: function () {
                let serialized = JSON.stringify({ "$type": "Averia.Core.Domain.Commands.SignIn, Averia.Core.Domain", "UserName": this.login });
                this.connection.send(serialized);
                this.signInComplete = true;
            },
            sendMessage: function () {
                let serialized = JSON.stringify({ "$type": "Averia.Core.Domain.Commands.UserMessage, Averia.Core.Domain", "Text": this.message });
                this.connection.send(serialized);
            }
        },
        created: function () {
            console.log("Starting connection to WebSocket Server");
            this.connection = new WebSocket("ws://127.0.0.1:1091");
            this.connection.parent = this;

            this.connection.onopen = function (event) {
                console.log("Successfully connected to the websocket server");
                console.log(event);
            };

            this.connection.onclose = function (event) {
                if (event.wasClean) {
                    console.log('Connection closed clear');
                } else {
                    console.log('Connection failed');
                }
            };

            this.connection.onmessage = function (event) {
                var data = JSON.parse(event.data);
                console.log(data)

                Array.prototype.forEach.call(data.Messages, child => {
                    this.parent.messages.push({
                        Author: child.Author,
                        Date: child.Date,
                        Text: child.Text,
                    });
                });
            };

            this.connection.onerror = function (error) {
                console.log("Ошибка " + error.message);
            };
        },
    };
</script>
