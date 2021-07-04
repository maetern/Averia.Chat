
<template>

<div id="app" class="form-group">
    <label>Логин</label>
    <input type="text" v-model="login" placeholder="Введите логин" class="form-control" /><br>
    <label>Message</label>
    <input type="text" v-model="Message" placeholder="Введите Message" class="form-control" /><br>
    <br>
    <div>
        <h3>Введенная информация</h3>
        <p>Логин: {{login}}</p>
        <p>Message: {{Message}}</p>
    </div>
      <button v-on:click="sendMessage()" class="btn btn-primary">Send Message</button>

<ul>
  <li v-for="(message, index) in messages" :key="index">
     {{ message.title }} - {{ message.author}} - {{ message.publishedAt}}
  </li>
</ul>

</div>



</template>

<script>
export default {
  name: 'App',
  data: function() {
    return {
      connection: null,
           login:'',
            Message:'222',
            products: ['80% cotton', '20% polyester', 'Gender-neutral'],
               items: [
      { message: 'Foo' },
      { message: 'Bar' }
    ],
               messages: [{
      title: 'How to do lists in Vue',
      author: 'Jane Doe',
      publishedAt: '2016-04-10'
    },
    {
      title: 'How to do lists in Vue',
      author: 'Jane Doe',
      publishedAt: '2016-04-10'
    }]
    }
  },
  methods: {
    sendMessage: function() {
      console.log(this.Message)
       this.connection.send(this.Message.toString());
    },
    addMessage: function(mess)
    {
 this.messages.push(   {
      title: mess,
      author: 'Jane Doe',
      publishedAt: '2016-04-10'
    })
    },
  },
  created: function() {

 
    console.log("Starting connection to WebSocket Server")
    this.connection = new WebSocket("wss://echo.websocket.org")
    this.connection.parent = this;
console.log(this.connection);
    this.connection.onmessage = function(event) {
      console.log(event);
      this.parent.addMessage(event.timeStamp);

      
    }

    this.connection.onopen = function(event) {
      console.log(event)
      console.log("Successfully connected to the echo websocket server...")
    }

  }
}
</script>
