

class Signup extends React.Component {
    render(){
        return (
            <div>
                <h1> test </h1>

                <Test text = "test" > </Test>
            </div>)
    }
}

const Test = function (props) {
    return <h1> {props.text}</h1>
}

React.render(<Signup />, document.getElementById("signup"));