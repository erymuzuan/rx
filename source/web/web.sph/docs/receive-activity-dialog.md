#ReceiveActivity

Receive Activity is an asyn activity that will instantiate and will sit waiting for the actual event to happen it's like [`ScreenActivity`](ScreenActivity.html)' without the user interface element.

While [`ScreenActivity`](ScreenActivity.html) is meant for human task interaction, [`ReceiveActivity`](ReceiveActivity.html) is meant for integration event at the API level only

##Properties
<table class="table table-condensed table-bordered">
    <thead>
<tr>
<th>Property</th>
<th>Description</th>
</tr>
</thead>
<tbody>
    <tr>
        <td>
            ArgumentPath
        </td>
        <td>
            The path to the variable where the received message will be stored. The message type received should be convertible to the message type specified in the variable
        </td>
    </tr>
</tbody></table>
